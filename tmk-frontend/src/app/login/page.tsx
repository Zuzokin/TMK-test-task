"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { login } from "@/services/userService";

interface LoginFormData {
  email: string;
  password: string;
}

export default function LoginPage() {
  const [formData, setFormData] = useState<LoginFormData>({ email: "", password: "" });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const router = useRouter();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      await login(formData); // Отправляем данные на сервер
      router.push("/"); // Перенаправляем на /dashboard
    } catch (err) {
      setError(err instanceof Error ? err.message : "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-lg mx-auto mt-8">
      <h1 className="text-2xl font-bold mb-6 text-center text-gray-800">Вход</h1>
      <form
        onSubmit={handleSubmit}
        className="space-y-6 bg-white p-6 rounded-lg shadow-md border"
      >
        {error && <p className="text-red-500 text-sm">{error}</p>}
  
        <div>
          <label htmlFor="email" className="block font-medium text-gray-700">
            Электронная почта
          </label>
          <input
            id="email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
          />
        </div>
  
        <div>
          <label htmlFor="password" className="block font-medium text-gray-700">
            Пароль
          </label>
          <input
            id="password"
            name="password"
            type="password"
            value={formData.password}
            onChange={handleChange}
            className="border rounded p-2 w-full mt-1 focus:ring-blue-500 focus:border-blue-500"
            required
          />
        </div>
  
        <div className="flex justify-between items-center">
          <button
            type="submit"
            disabled={loading}
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:opacity-50"
          >
            {loading ? "Вход..." : "Войти"}
          </button>
        </div>
      </form>
    </div>
  );
}
