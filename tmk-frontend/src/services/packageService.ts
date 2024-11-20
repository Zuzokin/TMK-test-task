import apiClient from '@/services/api';


export const getPackages = async () => {
  const response = await apiClient.get('/Packages');
  return response.data;
};

export const getPackageById = async (id: string) => {
  const response = await apiClient.get(`/Packages/${id}`);
  return response.data;
};

export const createPackage = async (data: any) => {
  const response = await apiClient.post('/Packages', data);
  return response.data;
};

// Обновление пакета
export const updatePackage = async (id: string, packageData: { number: string; date: string }) => {
  const response = await apiClient.put(`/Packages/${id}`, packageData);
  return response.data;
};

// Удаление пакета
export const deletePackage = async (id: string) => {
  const response = await apiClient.delete(`/Packages/${id}`);
  return response.data;
};

// Получение списка труб в пакете
export const getPackagePipes = async (packageId: string) => {
  const response = await apiClient.get(`/Packages/${packageId}/pipes`);
  return response.data;
};

export const addPipesToPackage = async (packageId: string, pipeIds: string[]) => {
  return await apiClient.post(`/Packages/${packageId}/add-pipes`, { pipeIds });
};


export const removePipeFromPackage = async (packageId: string, pipeId: string) => {
  const response = await apiClient.delete(`/Packages/${packageId}/remove-pipe/${pipeId}`);
  return response.data;
};
