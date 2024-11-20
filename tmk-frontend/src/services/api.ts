import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5090/api", // Backend в Docker доступен по порту 8080
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
