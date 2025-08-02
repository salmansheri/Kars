import AuctionCard from './AuctionCard'
import type { Auction } from '../../../types'
import { useSelectAuctions } from '@/hooks/use-select-auctions'

export const Listings = () => {
  const pageSize = 10
  const { data, isLoading } = useSelectAuctions(pageSize);


  if (isLoading) {
    return (
      <div className={"flex justify-center items-center"}>
        Loading...
      </div>
    )
  }



  return (
    <div className="grid grid-cols-1  md:grid-cols-3 lg:grid-cols-4 gap-6">


      {data?.results.map((auction: Auction) => (
            <AuctionCard isLoading={isLoading} key={auction.id} auction={auction} />
          ))}
    </div>
  )
}
