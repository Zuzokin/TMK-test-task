"use client";

import { useForm } from "react-hook-form";
import { useMutation, useQuery, useQueryClient } from "react-query";
import { getPackageById, updatePackage } from "@/services/packageService";
import { useRouter, useParams } from "next/navigation";
import React, { useEffect } from "react";

type FormData = {
  number: string;
  date: string; // Дата будет приходить как ISO-8601
};

export default function EditPackagePage() {
  const params = useParams();
  const router = useRouter();
  const queryClient = useQueryClient();

  const { data: packageData, isLoading, error } = useQuery(
    ["package", params?.id],
    () => getPackageById(params?.id as string),
    { enabled: !!params?.id }
  );

  const { register, handleSubmit, setValue, watch } = useForm<FormData>();

  useEffect(() => {
    if (packageData) {
      setValue("number", packageData.number);
      setValue(
        "date",
        packageData.date.split("T")[0] // Преобразуем дату в формат yyyy-MM-dd для поля date
      );
    }
  }, [packageData, setValue]);

  const mutation = useMutation(
    (data: FormData) =>
      updatePackage(params?.id as string, {
        ...data,
        date: new Date(data.date).toISOString(), // Преобразуем дату в ISO 8601
      }),
    {
      onSuccess: () => {
        queryClient.invalidateQueries("packages");
        router.push(`/packages/${params?.id}`);
      },
    }
  );

  const onSubmit = (data: FormData) => {
    mutation.mutate(data);
  };

  if (isLoading) return <div>Loading package details...</div>;
  if (error) return <div>Error loading package details.</div>;

  return (
    <div className="max-w-lg mx-auto mt-8">
      <h1 className="text-2xl font-bold mb-6 text-gray-800">Редактировать пакет</h1>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <label htmlFor="number" className="block font-medium">
            Номер пакета
          </label>
          <input
            id="number"
            type="text"
            {...register("number")}
            className="border rounded p-2 w-full"
            required
          />
        </div>
        <div>
          <label htmlFor="date" className="block font-medium">
            Дата
          </label>
          <input
            id="date"
            type="date"
            {...register("date")}
            className="border rounded p-2 w-full"
            required
          />
        </div>
        <button
          type="submit"
          disabled={mutation.isLoading}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          {mutation.isLoading ? "Обновление..." : "Обновить пакет"}
        </button>
      </form>
      <button
        onClick={() => router.push(`/packages/${params?.id}`)}
        className="mt-4 bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600"
      >
        Назад к пакету
      </button>
    </div>
  );
}
