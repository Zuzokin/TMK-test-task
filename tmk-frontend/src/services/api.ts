import axios from "axios";

const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || "http://localhost:8080/api", // По умолчанию "http://localhost:8080/api"
  withCredentials: true, // Включаем передачу cookies
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("tasty-cookies"); // Храним токен в localStorage
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});


export default apiClient;
