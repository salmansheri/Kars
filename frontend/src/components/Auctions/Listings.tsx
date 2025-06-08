
import AuctionCard from './AuctionCard'
import { useSelectAuctions } from '@/hooks/use-select-auctions'

export const Listings = () => {
  const pageSize = 10
  const { data, isLoading } = useSelectAuctions(pageSize)

  return (
    <div className="grid grid-cols-4 gap-6">
      {isLoading
        ? Array.from({ length: 8 }).map((_, index) => (
            <div
              key={index}
              className="w-full aspect-video bg-gray-300 rounded-lg animate-pulse"
            ></div>
          ))
        : data?.results.map((auction: any) => (
            <AuctionCard key={auction.id} auction={auction} />
          ))}
    </div>
  )
}
