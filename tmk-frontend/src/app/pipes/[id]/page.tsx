"use client";

import { useQuery, useMutation, useQueryClient } from "react-query";
import { getPipeById, deletePipe } from "@/services/pipeService";
import { useRouter, useParams } from "next/navigation";
import React from "react";

export default function PipeDetailsPage() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const params = useParams();
  const id = params?.id;

  if (!id || typeof id !== "string") {
    return <div className="text-red-500 text-lg">Invalid pipe ID.</div>;
  }

  const { data, isLoading, error } = useQuery(["pipe", id], () => getPipeById(id));

  const mutation = useMutation(() => deletePipe(id), {
    onSuccess: () => {
      queryClient.invalidateQueries("pipes");
      router.push("/pipes");
    },
  });

  if (isLoading) return <div className="text-gray-500 text-lg">Loading pipe details...</div>;
  if (error) return <div className="text-red-500 text-lg">Error loading pipe details.</div>;

  return (
    <div className="max-w-3xl mx-auto mt-8">
      <h1 className="text-3xl font-bold text-center text-gray-800 mb-6">Детали трубы</h1>
      <div className="bg-white shadow-md rounded-lg p-6 border">
        <div className="grid grid-cols-2 gap-4 mb-4">
          <div>
            <strong className="block text-gray-600">Маркировка:</strong>
            <p className="text-gray-800">{data.label}</p>
          </div>
          <div>
            <strong className="block text-gray-600">Статус:</strong>
            <p className={`text-lg ${data.isGood ? "text-green-600" : "text-red-600"}`}>
              {data.isGood ? "Годная" : "Дефектная"}
            </p>
          </div>
          <div>
            <strong className="block text-gray-600">Диаметр:</strong>
            <p className="text-gray-800">{data.diameter} мм</p>
          </div>
          <div>
            <strong className="block text-gray-600">Длина:</strong>
            <p className="text-gray-800">{data.length} м</p>
          </div>
          <div>
            <strong className="block text-gray-600">Вес:</strong>
            <p className="text-gray-800">{data.weight} кг</p>
          </div>
          <div>
            <strong className="block text-gray-600">Марка стали:</strong>
            <p className="text-gray-800">{data.steelGradeName}</p>
          </div>
        </div>
        <div>
          <strong className="block text-gray-600">Пакет:</strong>
          {data.packageNumber ? (
            <a
              href={`/packages/${data.packageId}`}
              className="text-blue-600 underline hover:text-blue-800"
            >
              {data.packageNumber}
            </a>
          ) : (
            <p className="text-gray-500">Не назначен</p>
          )}
        </div>
      </div>
  
      <div className="flex justify-between mt-6">
        <button
          onClick={() => router.push(`/pipes/${id}/edit`)}
          className="bg-yellow-500 text-white px-6 py-2 rounded hover:bg-yellow-600 shadow-md"
        >
          Редактировать трубу
        </button>
        <button
          onClick={() => {
            if (confirm("Вы уверены, что хотите удалить эту трубу?")) {
              mutation.mutate();
            }
          }}
          className="bg-red-500 text-white px-6 py-2 rounded hover:bg-red-600 shadow-md"
        >
          Удалить трубу
        </button>
        <button
          onClick={() => router.push(`/pipes`)}
          className="bg-gray-500 text-white px-6 py-2 rounded hover:bg-gray-600 shadow-md"
        >
          Назад к трубам
        </button>
      </div>
    </div>
  );
  
}
