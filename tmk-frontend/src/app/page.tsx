"use client";

import { useQuery, useQueryClient } from "react-query";
import { getPipeStatistics } from "@/services/pipeService";

export default function HomePage() {
  // Fetch статистики
  const { data, isLoading, error } = useQuery("pipeStatistics", getPipeStatistics);

  const queryClient = useQueryClient();

  if (isLoading) return <div>Загрузка статистики...</div>;
  if (error) return <div>Ошибка загрузки статистики.</div>;

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Статистика труб</h1>
      <div className="grid grid-cols-2 gap-4">
        <div className="p-4 border rounded bg-gray-100">
          <h2 className="text-lg font-semibold">Общее количество труб</h2>
          <p className="text-xl">{data?.totalCount || 0}</p>
        </div>
        <div className="p-4 border rounded bg-green-100">
          <h2 className="text-lg font-semibold">Годные трубы</h2>
          <p className="text-xl">{data?.goodCount || 0}</p>
        </div>
        <div className="p-4 border rounded bg-red-100">
          <h2 className="text-lg font-semibold">Дефектные трубы</h2>
          <p className="text-xl">{data?.defectiveCount || 0}</p>
        </div>
        <div className="p-4 border rounded bg-blue-100">
          <h2 className="text-lg font-semibold">Общий вес</h2>
          <p className="text-xl">{data?.totalWeight || 0} кг</p>
        </div>
        <button
          onClick={() => queryClient.invalidateQueries("pipeStatistics")}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 mt-4"
        >
          Обновить статистику
        </button>
      </div>
    </div>
  );
}
