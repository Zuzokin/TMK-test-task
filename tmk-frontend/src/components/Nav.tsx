"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useAuth } from "@/contexts/AuthContext";

export default function Nav() {
  const { user, logout } = useAuth(); // Подписываемся на контекст
  const pathname = usePathname();

  // Ссылки для авторизованных пользователей
  const privateLinks = [
    { name: "Статистика", href: "/" },
    { name: "Марки стали", href: "/steelgrades" },
    { name: "Трубы", href: "/pipes" },
    { name: "Пакеты", href: "/packages" },
  ];

  // Ссылки для неавторизованных пользователей
  const publicLinks = [
    { name: "Войти", href: "/login" },
    { name: "Регистрация", href: "/register" },
  ];

  return (
    <nav className="flex justify-between items-center max-w-7xl mx-auto">
      {/* Логотип */}
      <div className="text-lg font-bold">
        <Link href="/">TMK System</Link>
      </div>

      {/* Ссылки */}
      <div className="flex space-x-4">
        {(user ? privateLinks : publicLinks).map((link) => (
          <Link
            key={link.href}
            href={link.href}
            className={`px-4 py-2 rounded font-medium ${
              pathname === link.href
                ? "bg-blue-500 text-white"
                : "bg-gray-700 text-gray-300 hover:bg-blue-600 hover:text-white"
            }`}
          >
            {link.name}
          </Link>
        ))}

        {/* Кнопка "Выйти" для авторизованных пользователей */}
        {user && (
          <button
            onClick={logout}
            className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-md text-sm font-medium"
          >
            Выйти
          </button>
        )}
      </div>
    </nav>
  );
}
