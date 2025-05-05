'use client'; 
import React from 'react'
import { AiOutlineCar } from 'react-icons/ai';

export default function Navbar() {
  return (
    <header className="sticky top-0 z-50 flex justify-between p-5 items-center border-b bg-white dark:bg-gray-900 dark:border-gray-700 shadow-md">
      <div className="flex items-center gap-2 text-3xl font-semibold text-amber-500">
        <AiOutlineCar size={34} />
        <div>Kars Auctions</div>
      </div>
      <div>Search</div>
      <div>Login</div>
    </header>
  )
}
