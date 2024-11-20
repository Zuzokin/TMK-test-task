"use client";

import { useForm } from "react-hook-form";
import { useQuery, useMutation, useQueryClient } from "react-query";
import { getPipeById, updatePipe, getSteelGrades } from "@/services/pipeService";
import { useRouter, useParams } from "next/navigation";
import React from "react";

type PipeFormData = {
  label: string;
  isGood: string;
  diameter: string;
  length: string;
  weight: string;
  steelGradeId: string;
};

export default function EditPipePage() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const params = useParams();
  const id = params?.id;

  if (!id || typeof id !== "string") {
    return <div>Invalid pipe ID.</div>;
  }

  const { register, handleSubmit, setValue } = useForm<PipeFormData>();

  const { data: pipeData, isLoading: isLoadingPipe, error: errorPipe } = useQuery(
    ["pipe", id],
    () => getPipeById(id)
  );

  const { data: steelGrades, isLoading: isLoadingGrades, error: errorGrades } = useQuery(
    "steelGrades",
    getSteelGrades
  );

  const mutation = useMutation((updatedData: PipeFormData) => {
    const formattedData = {
      label: updatedData.label,
      isGood: updatedData.isGood === "true", // Преобразуем строку в boolean
      diameter: parseFloat(updatedData.diameter), // Преобразуем строку в число
      length: parseFloat(updatedData.length), // Преобразуем строку в число
      weight: parseFloat(updatedData.weight), // Преобразуем строку в число
      steelGradeId: updatedData.steelGradeId,
    };
    return updatePipe(id, formattedData);
  }, {
    onSuccess: () => {
      queryClient.invalidateQueries("pipes");
      router.push(`/pipes/${id}`);
    },
  });

  React.useEffect(() => {
    if (pipeData) {
      setValue("label", pipeData.label);
      setValue("isGood", pipeData.isGood);
      setValue("diameter", pipeData.diameter);
      setValue("length", pipeData.length);
      setValue("weight", pipeData.weight);
      setValue("steelGradeId", pipeData.steelGradeId || "");
    }
  }, [pipeData, setValue]);

  const onSubmit = (formData: PipeFormData) => {
    if (!formData.steelGradeId) {
      console.error("Steel Grade must be selected.");
      return;
    }
    mutation.mutate(formData);
  };

  if (isLoadingPipe || isLoadingGrades) return <div>Loading...</div>;
  if (errorPipe || errorGrades) return <div>Error loading pipe details or steel grades.</div>;

  return (
    <div className="max-w-lg mx-auto mt-8">
      <h1 className="text-2xl font-bold mb-6 text-center text-gray-800">Редактировать трубу</h1>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 bg-white p-6 rounded-lg shadow-md border">
        <div>
          <label htmlFor="label" className="block font-medium text-gray-700">Маркировка</label>
          <input
            id="label"
            {...register("label")}
            type="text"
            className="border rounded p-2 w-full mt-1"
            required
          />
        </div>
        <div>
          <label htmlFor="isGood" className="block font-medium text-gray-700">Статус</label>
          <select
            id="isGood"
            {...register("isGood")}
            className="border rounded p-2 w-full mt-1"
            required
          >
            <option value="true">Годная</option>
            <option value="false">Дефектная</option>
          </select>
        </div>
        <div>
          <label htmlFor="diameter" className="block font-medium text-gray-700">Диаметр (мм)</label>
          <input
            id="diameter"
            {...register("diameter")}
            type="number"
            className="border rounded p-2 w-full mt-1"
            required
          />
        </div>
        <div>
          <label htmlFor="length" className="block font-medium text-gray-700">Длина (м)</label>
          <input
            id="length"
            {...register("length")}
            type="number"
            className="border rounded p-2 w-full mt-1"
            required
          />
        </div>
        <div>
          <label htmlFor="weight" className="block font-medium text-gray-700">Вес (кг)</label>
          <input
            id="weight"
            {...register("weight")}
            type="number"
            className="border rounded p-2 w-full mt-1"
            required
          />
        </div>
        <div>
          <label htmlFor="steelGradeId" className="block font-medium text-gray-700">Марка стали</label>
          <select
            id="steelGradeId"
            {...register("steelGradeId")}
            className="border rounded p-2 w-full mt-1"
            required
          >
            <option value="">Выберите марку стали</option>
            {steelGrades?.map((grade: any) => (
              <option key={grade.id} value={grade.id}>{grade.name}</option>
            ))}
          </select>
        </div>
        <div className="flex justify-between">
          <button
            type="submit"
            disabled={mutation.isLoading}
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:opacity-50"
          >
            {mutation.isLoading ? "Сохранение..." : "Сохранить изменения"}
          </button>
          <button
            onClick={() => router.push(`/pipes/${id}`)}
            type="button"
            className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600"
          >
            Отменить
          </button>
        </div>
      </form>
      {mutation.isError && (
        <p className="text-red-500 mt-4 text-center">Не удалось обновить трубу. Пожалуйста, попробуйте снова.</p>
      )}
    </div>
  );
  
}
