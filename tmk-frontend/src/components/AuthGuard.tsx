"use client";

import { useAuth } from "@/contexts/AuthContext";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function AuthGuard({ children }: { children: React.ReactNode }) {
  const { user, loading } = useAuth(); // Получаем состояние пользователя из контекста
  const router = useRouter();

  useEffect(() => {
    // Если пользователь не авторизован и проверка завершена, перенаправляем на /login
    if (!loading && !user) {
      router.replace("/login");
    }
  }, [user, loading, router]);

  // Показываем загрузку, пока состояние авторизации не определено
  if (loading) {
    return <div>Загрузка...</div>;
  }

  // Если пользователь авторизован, отображаем защищённый контент
  return <>{children}</>;
}
