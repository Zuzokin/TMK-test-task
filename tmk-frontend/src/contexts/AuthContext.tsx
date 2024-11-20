"use client";

import { createContext, useContext, useEffect, useState } from "react";

interface AuthContextType {
  user: any | null;
  login: (userData: any) => void;
  logout: () => void;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<any | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Проверяем, есть ли токен в cookies
    const token = document.cookie.split("; ").find((c) => c.startsWith("tasty-cookies="));

    if (token) {
      const userData = { email: "user@example.com" }; // Симуляция данных пользователя
      setUser(userData);
    }

    setLoading(false);
  }, []);

  const login = (userData: any) => {
    setUser(userData); // Устанавливаем пользователя
    document.cookie = "tasty-cookies=valid; path=/;"; // Симуляция сохранения токена
  };

  const logout = () => {
    setUser(null); // Удаляем пользователя
    document.cookie = "tasty-cookies=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider");
  }
  return context;
}
