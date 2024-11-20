"use client";

import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "react-query";
import { createPackage } from "@/services/packageService";
import { useRouter } from "next/navigation";
import React from "react";

type FormData = {
  number: string;
  date: string;
};

export default function CreatePackagePage() {
  const { register, handleSubmit, setValue } = useForm<FormData>();
  const queryClient = useQueryClient();
  const router = useRouter();

  const mutation = useMutation(createPackage, {
    onSuccess: () => {
      queryClient.invalidateQueries("packages");
      router.push("/packages");
    },
  });

  // Устанавливаем текущую дату в формате yyyy-MM-dd
  React.useEffect(() => {
    const currentDate = new Date().toISOString().split("T")[0];
    setValue("date", currentDate);
  }, [setValue]);

  const onSubmit = (data: FormData) => {
    // Преобразуем дату в формат ISO 8601 перед отправкой
    const isoDate = new Date(data.date).toISOString();
    mutation.mutate({ ...data, date: isoDate });
  };

  return (
    <div className="max-w-lg mx-auto mt-8">
      <h1 className="text-2xl font-bold mb-6 text-center text-gray-800">
        Создать пакет
      </h1>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="space-y-6 bg-white p-6 rounded-lg shadow-md border"
      >
        <div>
          <label htmlFor="number" className="block font-medium text-gray-700">
            Номер пакета
          </label>
          <input
            id="number"
            type="text"
            {...register("number")}
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
          />
        </div>
        <div>
          <label htmlFor="date" className="block font-medium text-gray-700">
            Дата
          </label>
          <input
            id="date"
            type="date"
            {...register("date")}
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
          />
        </div>
        <div className="flex justify-between items-center">
          <button
            type="submit"
            disabled={mutation.isLoading}
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:opacity-50"
          >
            {mutation.isLoading ? "Создание..." : "Создать пакет"}
          </button>
          <button
            type="button"
            onClick={() => router.push("/packages")}
            className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600"
          >
            Назад к пакетам
          </button>
        </div>
      </form>
      {mutation.isError && (
        <p className="text-red-500 mt-4 text-center">
          Не удалось создать пакет. Пожалуйста, попробуйте снова.
        </p>
      )}
    </div>
  );
  
}
