"use client";

import { useQuery, useMutation, useQueryClient } from "react-query";
import {
  getPackageById,
  deletePackage,
  getPackagePipes,
  removePipeFromPackage,
} from "@/services/packageService";
import { useRouter } from "next/navigation";
import React from "react";
import Link from "next/link";

export default function PackageDetailsPage({ params: asyncParams }: { params: Promise<{ id: string }> }) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const [params, setParams] = React.useState<{ id: string } | null>(null);

  React.useEffect(() => {
    asyncParams.then((resolvedParams) => setParams(resolvedParams));
  }, [asyncParams]);

  const { data: packageData, isLoading: isPackageLoading, error: packageError } = useQuery(
    ["package", params?.id],
    () => getPackageById(params!.id),
    { enabled: !!params }
  );

  const { data: pipes, isLoading: isPipesLoading, error: pipesError } = useQuery(
    ["packagePipes", params?.id],
    () => getPackagePipes(params!.id),
    { enabled: !!params }
  );

  const deletePackageMutation = useMutation(() => deletePackage(params!.id), {
    onSuccess: () => {
      queryClient.invalidateQueries("packages");
      router.push("/packages");
    },
  });

  const removePipeMutation = useMutation(
    (pipeId: string) => removePipeFromPackage(params!.id, pipeId),
    {
      onSuccess: () => {
        queryClient.invalidateQueries(["packagePipes", params!.id]);
      },
    }
  );

  const handleDeletePackage = () => {
    if (confirm("Are you sure you want to delete this package?")) {
      deletePackageMutation.mutate();
    }
  };

  const handleRemovePipe = (pipeId: string) => {
    if (confirm("Are you sure you want to remove this pipe from the package?")) {
      removePipeMutation.mutate(pipeId);
    }
  };

  const handleAddPipes = () => {
    router.push(`/packages/${params!.id}/add-pipes`);
  };

  if (!params) return <div>Loading parameters...</div>;
  if (isPackageLoading || isPipesLoading) return <div>Loading package...</div>;
  if (packageError || pipesError) return <div>Error loading package details.</div>;

return (
  <div className="max-w-4xl mx-auto mt-8">
    <h1 className="text-3xl font-bold text-center mb-6 text-gray-800">Детали пакета</h1>
    <div className="bg-white shadow-md rounded-lg p-6 border mb-6">
      <p className="text-lg text-gray-600">
        <strong>Номер:</strong> {packageData.number}
      </p>
      <p className="text-lg text-gray-600">
        <strong>Дата:</strong> {new Date(packageData.date).toLocaleDateString()}
      </p>
    </div>

    <h2 className="text-2xl font-semibold text-gray-700 mb-4">Трубы в этом пакете</h2>
    {pipes && pipes.length > 0 ? (
      <ul className="space-y-4">
        {pipes.map((pipe: any) => (
          <li
            key={pipe.id}
            className="bg-gray-100 p-4 rounded-lg shadow hover:shadow-md transition"
          >
            <p className="text-gray-800">
              <strong>Маркировка:</strong> {pipe.label}
            </p>
            <p className="text-gray-600">
              <strong>Статус:</strong> {pipe.isGood ? "Годная" : "Дефектная"}
            </p>
            <p className="text-gray-600">
              <strong>Диаметр:</strong> {pipe.diameter} мм
            </p>
            <p className="text-gray-600">
              <strong>Вес:</strong> {pipe.weight} кг
            </p>
            <div className="flex justify-between items-center mt-4">
              <Link
                href={`/pipes/${pipe.id}`}
                className="text-blue-500 hover:underline"
              >
                Посмотреть детали трубы
              </Link>
              <button
                onClick={() => handleRemovePipe(pipe.id)}
                className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 shadow"
              >
                Удалить трубу
              </button>
            </div>
          </li>
        ))}
      </ul>
    ) : (
      <p className="text-gray-500">В этом пакете нет труб.</p>
    )}

    <div className="mt-6 flex justify-between">
      <button
        onClick={handleAddPipes}
        className="bg-green-500 text-white px-6 py-2 rounded hover:bg-green-600 shadow-md"
      >
        Добавить трубы
      </button>
      <button
        onClick={() => router.push(`/packages/${params.id}/edit`)}
        className="bg-yellow-500 text-white px-6 py-2 rounded hover:bg-yellow-600 shadow-md"
      >
        Редактировать пакет
      </button>
      <button
        onClick={handleDeletePackage}
        className="bg-red-500 text-white px-6 py-2 rounded hover:bg-red-600 shadow-md"
      >
        Удалить пакет
      </button>
      <button
        onClick={() => router.push("/packages")}
        className="bg-gray-500 text-white px-6 py-2 rounded hover:bg-gray-600 shadow-md"
      >
        Назад к пакетам
      </button>
    </div>
  </div>
);
}
