"use client";

import { useQuery, useMutation, useQueryClient } from "react-query";
import { getPipes, deletePipe } from "@/services/pipeService";
import { getSteelGrades } from "@/services/steelGradeService";
import { useRouter } from "next/navigation";
import { useState } from "react";

interface Pipe {
  id: string;
  label: string;
  diameter: number;
  length: number;
  weight: number;
  isGood: boolean;
  steelGradeName: string; // Имя марки стали
}

interface SteelGrade {
  id: string;
  name: string;
}

export default function PipesPage() {
  const queryClient = useQueryClient();
  const router = useRouter();
  const [filters, setFilters] = useState({
    diameter: "",
    minWeight: "",
    maxWeight: "",
    isGood: "",
    steelGradeId: "",
    minLength: "",
    maxLength: "",
    minDiameter: "",
    maxDiameter: "",
    inPackage: "",
  });
  
  const parsedFilters = {
    steelGradeId: filters.steelGradeId || undefined,
    isGood: filters.isGood === "" ? undefined : filters.isGood === "true",
    minWeight: filters.minWeight ? parseFloat(filters.minWeight) : undefined,
    maxWeight: filters.maxWeight ? parseFloat(filters.maxWeight) : undefined,
    minLength: filters.minLength ? parseFloat(filters.minLength) : undefined,
    maxLength: filters.maxLength ? parseFloat(filters.maxLength) : undefined,
    minDiameter: filters.minDiameter ? parseFloat(filters.minDiameter) : undefined,
    maxDiameter: filters.maxDiameter ? parseFloat(filters.maxDiameter) : undefined,
    notInPackage: filters.inPackage === "false" ? true : filters.inPackage === "true" ? false : undefined,
  };

  const { data: pipes, isLoading, error } = useQuery(["pipes", parsedFilters], () =>
    getPipes(parsedFilters)
  );

  // Fetch steel grades for dropdown
  const { data: steelGrades } = useQuery<SteelGrade[]>("steelGrades", getSteelGrades);

  const deleteMutation = useMutation((id: string) => deletePipe(id), {
    onSuccess: () => {
      queryClient.invalidateQueries("pipes");
    },
  });

  const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFilters((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleDelete = (id: string) => {
    if (confirm("Are you sure you want to delete this pipe?")) {
      deleteMutation.mutate(id);
    }
  };

  if (isLoading) return <div>Loading pipes...</div>;
  if (error) return <div>Error loading pipes.</div>;

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Список труб</h1>
  
      {/* Фильтры */}
      <div className="mb-4 grid grid-cols-2 gap-4">
        <div>
          <label htmlFor="minWeight" className="block font-medium">
            Минимальный вес (кг)
          </label>
          <input
            id="minWeight"
            name="minWeight"
            type="number"
            value={filters.minWeight}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          />
        </div>
        <div>
          <label htmlFor="maxWeight" className="block font-medium">
            Максимальный вес (кг)
          </label>
          <input
            id="maxWeight"
            name="maxWeight"
            type="number"
            value={filters.maxWeight}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          />
        </div>
        <div>
          <label htmlFor="minLength" className="block font-medium">
            Минимальная длина (м)
          </label>
          <input
            id="minLength"
            name="minLength"
            type="number"
            value={filters.minLength}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          />
        </div>
        <div>
          <label htmlFor="maxLength" className="block font-medium">
            Максимальная длина (м)
          </label>
          <input
            id="maxLength"
            name="maxLength"
            type="number"
            value={filters.maxLength}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          />
        </div>
        <div>
          <label htmlFor="minDiameter" className="block font-medium">
            Минимальный диаметр (мм)
          </label>
          <input
            id="minDiameter"
            name="minDiameter"
            type="number"
            value={filters.minDiameter}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          />
        </div>
        <div>
          <label htmlFor="maxDiameter" className="block font-medium">
            Максимальный диаметр (мм)
          </label>
          <input
            id="maxDiameter"
            name="maxDiameter"
            type="number"
            value={filters.maxDiameter}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          />
        </div>
  
        <div>
          <label htmlFor="isGood" className="block font-medium">
            Статус
          </label>
          <select
            id="isGood"
            name="isGood"
            value={filters.isGood}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          >
            <option value="">Все</option>
            <option value="true">Годные</option>
            <option value="false">Дефектные</option>
          </select>
        </div>
        <div>
          <label htmlFor="steelGradeId" className="block font-medium">
            Марка стали
          </label>
          <select
            id="steelGradeId"
            name="steelGradeId"
            value={filters.steelGradeId}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          >
            <option value="">Все</option>
            {steelGrades?.map((grade) => (
              <option key={grade.id} value={grade.id}>
                {grade.name}
              </option>
            ))}
          </select>
        </div>
        <div>
          <label htmlFor="inPackage" className="block font-medium">
            В пакете
          </label>
          <select
            id="inPackage"
            name="inPackage"
            value={filters.inPackage}
            onChange={handleFilterChange}
            className="border rounded p-2 w-full"
          >
            <option value="">Все</option>
            <option value="true">В пакете</option>
            <option value="false">Не в пакете</option>
          </select>
        </div>      
    </div>
  
      
      {/* Кнопка добавления трубы */}
      <button
        className="mt-4 bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
        onClick={() => router.push("/pipes/create")}
      >
        Добавить трубу
      </button>
  
      {/* Таблица */}
      <table className="table-auto w-full border-collapse border border-gray-300">
        <thead>
          <tr className="bg-gray-100">
            <th className="border border-gray-300 p-2">Маркировка</th>
            <th className="border border-gray-300 p-2">Диаметр</th>
            <th className="border border-gray-300 p-2">Длина</th>
            <th className="border border-gray-300 p-2">Вес</th>
            <th className="border border-gray-300 p-2">Марка стали</th>
            <th className="border border-gray-300 p-2">Статус</th>
            <th className="border border-gray-300 p-2">Действия</th>
          </tr>
        </thead>
        <tbody>
          {pipes.length > 0 ? (
            pipes.map((pipe: Pipe) => (
              <tr
                key={pipe.id}
                className={pipe.isGood ? "" : "bg-red-100"}
              >
                <td className="border border-gray-300 p-2">{pipe.label}</td>
                <td className="border border-gray-300 p-2">{pipe.diameter} мм</td>
                <td className="border border-gray-300 p-2">{pipe.length} м</td>
                <td className="border border-gray-300 p-2">{pipe.weight} кг</td>
                <td className="border border-gray-300 p-2">{pipe.steelGradeName}</td>
                <td className="border border-gray-300 p-2">
                  {pipe.isGood ? "Годная" : "Дефектная"}
                </td>
                <td className="border border-gray-300 p-2">
                  <button
                    className="bg-blue-500 text-white px-2 py-1 rounded hover:bg-blue-600 mr-2"
                    onClick={() => router.push(`/pipes/${pipe.id}/edit`)}
                  >
                    Редактировать
                  </button>
                  <button
                    className="bg-red-500 text-white px-2 py-1 rounded hover:bg-red-600 mr-2"
                    onClick={() => handleDelete(pipe.id)}
                  >
                    Удалить
                  </button>
                  <button
                    className="bg-gray-500 text-white px-2 py-1 rounded hover:bg-gray-600 mr-2"
                    onClick={() => router.push(`/pipes/${pipe.id}`)}
                  >
                    Просмотреть
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={7} className="text-center p-4">
                Трубы не соответствуют выбранным фильтрам.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
  
}
