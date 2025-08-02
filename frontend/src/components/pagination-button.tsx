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

}

export const PaginationButton = ({auctions, page}: PaginationButtonProps) => {
  const navigate = Route.useNavigate();
  return (
    <Pagination>
      <PaginationContent>
        <PaginationItem>
          <PaginationPrevious href="#" />
        </PaginationItem>
        <PaginationItem>
          {auctions.map((item: Auction, index: number) => (
            <PaginationLink onClick={() => navigate({ search: prev => ({ ...prev, page: index + 1}) })}   key={item.id}    >{index + 1}</PaginationLink>

          ))}

        </PaginationItem>

        <PaginationItem>
          <PaginationNext onClick={() => navigate({ search: prev => ({ ...prev, page: page + 1}) })} href="#" />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  )
}

