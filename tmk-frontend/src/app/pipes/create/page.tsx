"use client";

import { useForm } from "react-hook-form";
import { useQuery, useMutation, useQueryClient } from "react-query";
import { createPipe, getSteelGrades } from "@/services/pipeService";
import { useRouter } from "next/navigation";
import React, { useEffect } from "react";

type FormData = {
  label: string;
  isGood: string; // Используем string, чтобы соответствовать select
  diameter: string; // Из формы приходят строки
  length: string; // Из формы приходят строки
  weight: string; // Из формы приходят строки
  steelGradeId: string;
};

export default function CreatePipePage() {
  const { register, handleSubmit, setValue } = useForm<FormData>();
  const queryClient = useQueryClient();
  const router = useRouter();

  // Загружаем список категорий стали
  const { data: steelGrades, isLoading: loadingGrades, error } = useQuery(
    "steelGrades",
    getSteelGrades
  );

  const mutation = useMutation(
    (data: FormData) => {
      // Преобразование данных перед отправкой
      const formattedData = {
        label: data.label,
        isGood: data.isGood === "true", // Преобразуем строку в boolean
        diameter: parseFloat(data.diameter), // Преобразуем строку в число
        length: parseFloat(data.length), // Преобразуем строку в число
        weight: parseFloat(data.weight), // Преобразуем строку в число
        steelGradeId: data.steelGradeId,
      };
      return createPipe(formattedData);
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries("pipes");
        router.push("/pipes");
      },
    }
  );

  const onSubmit = (data: FormData) => {
    if (!data.steelGradeId) {
      console.error("Steel Grade must be selected.");
      return;
    }
    mutation.mutate(data);
  };

  if (error) return <div>Error loading Steel Grades.</div>;

  return (
    <div className="max-w-lg mx-auto mt-8">
      <h1 className="text-2xl font-bold mb-6 text-center text-gray-800">
        Создать трубу
      </h1>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="space-y-6 bg-white p-6 rounded-lg shadow-md border"
      >
        <div>
          <label htmlFor="label" className="block font-medium text-gray-700">
            Маркировка
          </label>
          <input
            id="label"
            {...register("label")}
            type="text"
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
          />
        </div>
        <div>
          <label htmlFor="isGood" className="block font-medium text-gray-700">
            Статус
          </label>
          <select
            id="isGood"
            {...register("isGood")}
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
            defaultValue=""
          >
            <option value="" disabled>
              Выберите статус
            </option>
            <option value="true">Годная</option>
            <option value="false">Дефектная</option>
          </select>
        </div>
        <div>
          <label htmlFor="diameter" className="block font-medium text-gray-700">
            Диаметр (мм)
          </label>
          <input
            id="diameter"
            {...register("diameter")}
            type="number"
            step="0.01"
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
          />
        </div>
        <div>
          <label htmlFor="length" className="block font-medium text-gray-700">
            Длина (м)
          </label>
          <input
            id="length"
            {...register("length")}
            type="number"
            step="0.01"
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
          />
        </div>
        <div>
          <label htmlFor="weight" className="block font-medium text-gray-700">
            Вес (кг)
          </label>
          <input
            id="weight"
            {...register("weight")}
            type="number"
            step="0.01"
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
          />
        </div>
        <div>
          <label
            htmlFor="steelGradeId"
            className="block font-medium text-gray-700"
          >
            Марка стали
          </label>
          <select
            id="steelGradeId"
            {...register("steelGradeId")}
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
            defaultValue=""
          >
            <option value="" disabled>
              {loadingGrades ? "Загрузка..." : "Выберите марку стали"}
            </option>
            {steelGrades?.map((grade: any) => (
              <option key={grade.id} value={grade.id}>
                {grade.name}
              </option>
            ))}
          </select>
        </div>
        <div className="flex justify-between items-center">
          <button
            type="submit"
            disabled={mutation.isLoading}
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:opacity-50"
          >
            {mutation.isLoading ? "Создание..." : "Создать трубу"}
          </button>
          <button
            type="button"
            onClick={() => router.push("/pipes")}
            className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600"
          >
            Назад к трубам
          </button>
        </div>
      </form>
      {mutation.isError && (
        <p className="text-red-500 mt-4 text-center">
          Не удалось создать трубу. Пожалуйста, попробуйте снова.
        </p>
      )}
    </div>
  );
}
