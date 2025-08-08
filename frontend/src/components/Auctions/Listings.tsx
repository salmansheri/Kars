import AuctionCard from './AuctionCard'
import type { Auction } from '../../../types'
import { useSelectAuctions } from '@/hooks/use-select-auctions'
import { PaginationButton } from '@/components/pagination-button.tsx';

interface ListingsProps {
  page: number;
}

export const Listings = ({page} : ListingsProps) => {
  const pageSize = 10

  const { data, isLoading } = useSelectAuctions(pageSize, page);


  if (isLoading) {
    return (
      <div className={"flex justify-center items-center"}>
        Loading...
      </div>
    )
  }

  console.log("data" + data);



  return (
    <>

     <div className="grid grid-cols-1  md:grid-cols-3 lg:grid-cols-4 gap-6">


      {data && data.results.map((auction: Auction) => (
        <AuctionCard isLoading={isLoading} key={auction.id} auction={auction} />
      ))}

    </div>
      <div className="my-10">
        {data && (
          <PaginationButton page={page}  auctions={data.results} pageSize={pageSize}  />

        )}

      </div>

    </>

  )
}
