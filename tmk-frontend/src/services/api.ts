import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:8080/api", // Backend в Docker доступен по порту 8080  http://localhost:5690/api "http://pipemanager.web:8080"
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
