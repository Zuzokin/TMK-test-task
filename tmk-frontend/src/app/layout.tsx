"use client";

import "./globals.css";
import { QueryClient, QueryClientProvider } from "react-query";
import { useState } from "react";
import { AuthProvider } from "@/contexts/AuthContext";
import Nav from "@/components/Nav"; // Импортируем Nav

export default function RootLayout({ children }: { children: React.ReactNode }) {
  const [queryClient] = useState(() => new QueryClient());

  return (
    <html lang="ru">
      <body>
        <AuthProvider>
          <QueryClientProvider client={queryClient}>
            {/* Навигация */}
            <header className="bg-gray-900 text-white p-4">
              <Nav /> {/* Добавляем компонент навигации */}
            </header>

            {/* Основное содержимое */}
            <main className="p-6">{children}</main>
          </QueryClientProvider>
        </AuthProvider>
      </body>
    </html>
  );
}
