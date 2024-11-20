"use client";

import { useQuery, useMutation, useQueryClient } from "react-query";
import { getPackages, deletePackage } from "@/services/packageService";
import Link from "next/link";

export default function PackagesPage() {
  const queryClient = useQueryClient();

  const { data: packages = [], isLoading, error } = useQuery("packages", getPackages);

  const deleteMutation = useMutation((id: string) => deletePackage(id), {
    onSuccess: () => {
      queryClient.invalidateQueries("packages");
    },
  });

  const handleDelete = (id: string) => {
    if (confirm("Are you sure you want to delete this package?")) {
      deleteMutation.mutate(id);
    }
  };

  if (isLoading) return <div className="text-gray-500">Loading packages...</div>;
  if (error) return <div className="text-red-500">Error loading packages.</div>;

  return (
    <div className="max-w-4xl mx-auto mt-8">
      <h1 className="text-3xl font-bold mb-6 text-center text-gray-800">Пакеты</h1>
      <div className="mb-6 text-right">
        <Link
          href="/packages/create"
          className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
        >
          Создать новый пакет
        </Link>
      </div>
  
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
        {packages.map((pkg: any) => (
          <div
            key={pkg.id}
            className="border rounded-lg shadow-md p-4 bg-white hover:shadow-lg transition"
          >
            <h2 className="text-lg font-bold text-gray-700 mb-2">
              Номер пакета: {pkg.number}
            </h2>
            <p className="text-sm text-gray-500 mb-4">
              Дата: {new Date(pkg.date).toLocaleDateString()}
            </p>
            <div className="flex space-x-2">
              <Link
                href={`/packages/${pkg.id}`}
                className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
              >
                Просмотреть
              </Link>
              <Link
                href={`/packages/${pkg.id}/edit`}
                className="bg-yellow-500 text-white px-4 py-2 rounded hover:bg-yellow-600"
              >
                Редактировать
              </Link>
              <button
                onClick={() => handleDelete(pkg.id)}
                className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
              >
                Удалить
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
