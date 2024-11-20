import apiClient from "./api";

export const registerUser = async (data: { email: string; password: string }) => {
  const response = await apiClient.post("/Users/register", data);
  return response.data;
};

interface LoginRequest {
  email: string;
  password: string;
}

export const login = async (credentials: LoginRequest): Promise<void> => {
  try {
    // Выполняем запрос на сервер
    const response = await apiClient.post("/Users/login", credentials, {
      withCredentials: true, // Передача cookies
    });

    // Проверяем статус ответа через response.status, если требуется
    if (response.status !== 200) {
      throw new Error("Login failed");
    }
  } catch (error: any) {
    // Обработка ошибок
    const errorMessage =
      error.response?.data?.message || "Failed to login. Please try again.";
    throw new Error(errorMessage);
  }
};