import apiClient from "./api";
import axios from "axios";

export const getPipes = async (filters: {
  steelGradeId?: string;
  isGood?: boolean;
  minWeight?: number;
  maxWeight?: number;
  minLength?: number;
  maxLength?: number;
  minDiameter?: number;
  maxDiameter?: number;
  notInPackage?: boolean;
} = {}) => {
  const response = await apiClient.get("/Pipes/filter", {
    params: filters, // Передаем фильтры как параметры запроса
  });
  return response.data;
};

  
  export const createPipe = async (data: {
    label: string;
    isGood: boolean;
    diameter: number;
    length: number;
    weight: number;
    steelGradeId: string;
  }) => {
    const response = await apiClient.post("/Pipes", data);
    return response.data;
  };
  
  
  export const getSteelGrades = async () => {
    const response = await apiClient.get("/SteelGrades");
    return response.data;
  };

  export const getPipeById = async (id: string) => {
    const response = await apiClient.get(`/Pipes/${id}`);
    return response.data;    
  };

  export type PipeFormData = {
    label: string;
    isGood: boolean;
    diameter: number;
    length: number;
    weight: number;
  };  

  export const updatePipe = async (id: string, data: PipeFormData) => {
    const response = await apiClient.put(`/Pipes/${id}`, data);
    return response.data;
  };

  export const deletePipe = async (id: string) => {
    const response = await apiClient.delete(`/Pipes/${id}`);
    return response.data;
  };
  
  export const getPipeStatistics = async () => {
    const response = await apiClient.get("/Pipes/statistics");
    return response.data;
  };
  
  
  