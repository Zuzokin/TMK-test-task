import apiClient from "./api";

// Получение списка марок стали
export const getSteelGrades = async () => {
  const response = await apiClient.get("/SteelGrades");
  return response.data;
};

// Создание новой марки стали
export const createSteelGrade = async (data: { name: string }) => {
  const response = await apiClient.post("/SteelGrades", data);
  return response.data;
};

// Обновление существующей марки стали
export const updateSteelGrade = async (data: { id: string; name: string }) => {
  const { id, name } = data;
  const response = await apiClient.put(`/SteelGrades/${id}`, { name });
  return response.data;
};

// Удаление марки стали
export const deleteSteelGrade = async (id: string) => {
  const response = await apiClient.delete(`/SteelGrades/${id}`);
  return response.data;
};
