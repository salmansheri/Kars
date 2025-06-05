import { AiOutlineCar } from 'react-icons/ai'

export default function Header() {
  return (
    <header className="flex items-center justify-between sticky top-0 z-50 border-b border-gray-200/20 p-5 shadow-md">
      <div className="flex items-center gap-2 text-3xl font-semibold text-red-500">
        <AiOutlineCar size={34} />
        <div>Kars</div>
      </div>
      <div>Seach</div>
      <div>Login</div>
    </header>
  )
}
