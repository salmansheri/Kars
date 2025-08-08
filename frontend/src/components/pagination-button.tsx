import type { Auction } from '../../types'
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination";
import { Route } from '@/routes'

interface  PaginationButtonProps {
  auctions: Array<Auction>;
  page: number;
  pageSize: number

}

export const PaginationButton = ({auctions, page, pageSize}: PaginationButtonProps) => {
  const navigate = Route.useNavigate();
  const totalAuctionCount = auctions.length;
  const totalPages = Math.ceil(totalAuctionCount / pageSize);

  const handlePaginationClick = (index: number) => {
    navigate({ search: prev => ({...prev, page: index + 1})});

  }

  const handleNextPageClick = () => {
    navigate({search: prev => ({...prev, page: page + 1})});
  }

  const handlePreviousPageClick = () => {
    if (page > 1) {
      navigate({search: prev => ({...prev, page: page - 1})});

    }

  }
  return (
    <Pagination>
      <PaginationContent>
        <PaginationItem>
          <PaginationPrevious aria-disabled={page === 1} href="#" onClick={handlePreviousPageClick} />
        </PaginationItem>
        <PaginationItem>
          {Array.from({length: totalPages }).map((_, index: number) => (
            <PaginationLink onClick={() => handlePaginationClick(index)}   key={index}    >{index + 1}</PaginationLink>

          ))}




        </PaginationItem>

        <PaginationItem>
          <PaginationNext onClick={handleNextPageClick} href="#" />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  )
}

