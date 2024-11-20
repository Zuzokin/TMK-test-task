"use client";

import { useState } from "react";

interface LoginLayoutProps {
  loginContent: React.ReactNode;
  registerContent: React.ReactNode;
}

export default function LoginLayout({ loginContent, registerContent }: LoginLayoutProps) {
  const [activeTab, setActiveTab] = useState<"login" | "register">("login");

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100">
      <div className="w-full max-w-md p-6 bg-white shadow-md rounded-md">
        {/* Переключатель вкладок */}
        <div className="flex justify-around mb-6">
          <button
            onClick={() => setActiveTab("login")}
            className={`px-4 py-2 font-medium ${
              activeTab === "login"
                ? "border-b-2 border-blue-500 text-blue-500"
                : "text-gray-500"
            }`}
          >
            Login
          </button>
          <button
            onClick={() => setActiveTab("register")}
            className={`px-4 py-2 font-medium ${
              activeTab === "register"
                ? "border-b-2 border-blue-500 text-blue-500"
                : "text-gray-500"
            }`}
          >
            Register
          </button>
        </div>

        {/* Содержимое вкладки */}
        <div>{activeTab === "login" ? loginContent : registerContent}</div>
      </div>
    </div>
  );
}
