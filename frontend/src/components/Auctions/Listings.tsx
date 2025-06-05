import { FiLoader } from 'react-icons/fi'
import { useSelectAuctions } from '@/hooks/use-select-auctions'

export const Listings = () => {
  const { data, isLoading } = useSelectAuctions()

  if (isLoading) {
    return (
      <div className="w-full h-full flex items-center justify-center">
        <FiLoader className="animate-spin" size={30} />
      </div>
    )
  }
  return <div>{JSON.stringify(data)}</div>
}
