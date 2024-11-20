"use client";

import { useQuery, useMutation, useQueryClient } from "react-query";
import { getSteelGrades, createSteelGrade, updateSteelGrade, deleteSteelGrade } from "@/services/steelGradeService";
import { useState } from "react";

interface SteelGrade {
  id: string;
  name: string;
}

export default function SteelGradesPage() {
  const queryClient = useQueryClient();
  const [editingGrade, setEditingGrade] = useState<SteelGrade | null>(null);
  const [newGradeName, setNewGradeName] = useState("");

  // Загрузка списка марок стали
  const { data: steelGrades, isLoading, error } = useQuery("steelGrades", getSteelGrades);

  // Мутации для добавления, редактирования и удаления
  const createMutation = useMutation(createSteelGrade, {
    onSuccess: () => {
      queryClient.invalidateQueries("steelGrades");
      setNewGradeName("");
    },
  });

  const updateMutation = useMutation(updateSteelGrade, {
    onSuccess: () => {
      queryClient.invalidateQueries("steelGrades");
      setEditingGrade(null);
    },
  });

  const deleteMutation = useMutation(deleteSteelGrade, {
    onSuccess: () => {
      queryClient.invalidateQueries("steelGrades");
    },
  });

  // Обработчик добавления марки стали
  const handleAddSteelGrade = () => {
    if (newGradeName.trim() === "") {
      alert("Название марки стали не может быть пустым.");
      return;
    }
    createMutation.mutate({ name: newGradeName });
  };

  // Обработчик редактирования марки стали
  const handleEditSteelGrade = () => {
    if (editingGrade && editingGrade.name.trim() !== "") {
      updateMutation.mutate({ id: editingGrade.id, name: editingGrade.name });
    } else {
      alert("Название марки стали не может быть пустым.");
    }
  };

  // Обработчик удаления марки стали
  const handleDeleteSteelGrade = (id: string) => {
    if (confirm("Вы уверены, что хотите удалить эту марку стали?")) {
      deleteMutation.mutate(id);
    }
  };

  if (isLoading) return <div>Загрузка списка марок стали...</div>;
  if (error) return <div>Ошибка при загрузке данных.</div>;

  return (
    <div className="max-w-3xl mx-auto mt-8">
      <h1 className="text-2xl font-bold mb-6 text-center">Справочник марок стали</h1>

      <div className="mb-6">
        <h2 className="text-lg font-medium mb-2">Добавить новую марку стали</h2>
        <div className="flex space-x-2">
          <input
            type="text"
            value={newGradeName}
            onChange={(e) => setNewGradeName(e.target.value)}
            className="border rounded p-2 flex-grow"
            placeholder="Название марки стали"
          />
          <button
            onClick={handleAddSteelGrade}
            className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
            disabled={createMutation.isLoading}
          >
            Добавить
          </button>
        </div>
      </div>

      <table className="table-auto w-full border-collapse border border-gray-300">
        <thead>
          <tr className="bg-gray-100">
            <th className="border border-gray-300 p-2">Название</th>
            <th className="border border-gray-300 p-2">Действия</th>
          </tr>
        </thead>
        <tbody>
          {steelGrades?.map((grade: SteelGrade) => (
            <tr key={grade.id}>
              <td className="border border-gray-300 p-2">
                {editingGrade?.id === grade.id ? (
                  <input
                    type="text"
                    value={editingGrade.name}
                    onChange={(e) =>
                      setEditingGrade({ ...editingGrade, name: e.target.value })
                    }
                    className="border rounded p-2 w-full"
                  />
                ) : (
                  grade.name
                )}
              </td>
              <td className="border border-gray-300 p-2 flex space-x-2">
                {editingGrade?.id === grade.id ? (
                  <>
                    <button
                      onClick={handleEditSteelGrade}
                      className="bg-blue-500 text-white px-2 py-1 rounded hover:bg-blue-600"
                      disabled={updateMutation.isLoading}
                    >
                      Сохранить
                    </button>
                    <button
                      onClick={() => setEditingGrade(null)}
                      className="bg-gray-500 text-white px-2 py-1 rounded hover:bg-gray-600"
                    >
                      Отмена
                    </button>
                  </>
                ) : (
                  <>
                    <button
                      onClick={() => setEditingGrade(grade)}
                      className="bg-yellow-500 text-white px-2 py-1 rounded hover:bg-yellow-600"
                    >
                      Редактировать
                    </button>
                    <button
                      onClick={() => handleDeleteSteelGrade(grade.id)}
                      className="bg-red-500 text-white px-2 py-1 rounded hover:bg-red-600"
                      disabled={deleteMutation.isLoading}
                    >
                      Удалить
                    </button>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
