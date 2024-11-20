import apiClient from "./api";

export const login = async (credentials: { email: string; password: string }) => {
  const response = await apiClient.post("/Users/login", credentials, { withCredentials: true });
  const token = response.data.token;

  // Сохраняем токен в localStorage для AuthContext
  localStorage.setItem("tasty-cookies", token);

  return token;
};

export const register = async (data: { email: string; password: string }) => {
  const response = await apiClient.post("/Users/register", data);
  return response.data;
};

export const getUser = async () => {
  const response = await apiClient.get("/Users/me");
  return response.data;
};
