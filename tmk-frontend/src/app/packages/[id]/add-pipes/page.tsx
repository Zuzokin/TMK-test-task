"use client";

import { useQuery, useMutation, useQueryClient } from "react-query";
import { addPipesToPackage } from "@/services/packageService";
import { getPipes } from "@/services/pipeService";
import { useRouter, useParams } from "next/navigation";
import React, { useState } from "react";

export default function AddPipesToPackagePage() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const params = useParams();
  const packageId = params?.id;

  const [selectedPipes, setSelectedPipes] = useState<string[]>([]);

  const { data: pipes, isLoading, error } = useQuery(
    "pipes",
    () => getPipes({ notInPackage: true }), // Fetch only pipes not in any package
    { refetchOnWindowFocus: false }
  );

  const addPipesMutation = useMutation(
    () => addPipesToPackage(params?.id as string, selectedPipes), // Передаём массив напрямую
    {
      onSuccess: () => {
        queryClient.invalidateQueries(["packagePipes", params?.id]);
        router.push(`/packages/${params?.id}`);
      },
    }
  );
  
  

  const handleCheckboxChange = (pipeId: string) => {
    setSelectedPipes((prev) =>
      prev.includes(pipeId)
        ? prev.filter((id) => id !== pipeId)
        : [...prev, pipeId]
    );
  };

  const handleAddPipes = () => {
    if (selectedPipes.length === 0) {
      alert("Please select at least one pipe to add.");
      return;
    }
    addPipesMutation.mutate(); // Здесь вызывается функция без передачи параметров
  };
  

  if (isLoading) return <div>Loading available pipes...</div>;
  if (error) return <div>Error loading pipes. Please try again later.</div>;

  return (
    <div className="max-w-4xl mx-auto mt-8">
      <h1 className="text-3xl font-bold text-center mb-6 text-gray-800">
        Добавить трубы в пакет
      </h1>
  
      <div className="bg-white shadow-md rounded-lg p-6 border">
        <h2 className="text-2xl font-semibold text-gray-700 mb-4">
          Доступные трубы
        </h2>
        {pipes && pipes.length > 0 ? (
          <ul className="space-y-4">
            {pipes.map((pipe: any) => (
              <li
                key={pipe.id}
                className="flex items-center justify-between bg-gray-100 p-4 rounded-lg shadow hover:shadow-md transition"
              >
                <div>
                  <p className="text-gray-800">
                    <strong>Маркировка:</strong> {pipe.label}
                  </p>
                  <p className="text-gray-600">
                    <strong>Статус:</strong>{" "}
                    {pipe.isGood ? "Годная" : "Дефектная"}
                  </p>
                  <p className="text-gray-600">
                    <strong>Диаметр:</strong> {pipe.diameter} мм
                  </p>
                  <p className="text-gray-600">
                    <strong>Вес:</strong> {pipe.weight} кг
                  </p>
                </div>
                <input
                  type="checkbox"
                  checked={selectedPipes.includes(pipe.id)}
                  onChange={() => handleCheckboxChange(pipe.id)}
                  className="w-5 h-5 cursor-pointer"
                />
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-500">Нет доступных труб для добавления.</p>
        )}
      </div>
  
      <div className="mt-6 flex justify-between">
        <button
          onClick={handleAddPipes}
          className="bg-green-500 text-white px-6 py-2 rounded hover:bg-green-600 shadow-md"
          disabled={addPipesMutation.isLoading}
        >
          {addPipesMutation.isLoading ? "Добавление труб..." : "Добавить выбранные трубы"}
        </button>
        <button
          onClick={() => router.push(`/packages/${packageId}`)}
          className="bg-gray-500 text-white px-6 py-2 rounded hover:bg-gray-600 shadow-md"
        >
          Назад к пакету
        </button>
      </div>
    </div>
  );
  
}
