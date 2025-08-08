import { useQuery } from '@tanstack/react-query'
import axios from 'axios'
import type { Auction, PagedResult } from '../../types'
import { BackendAppUrl } from '@/lib/utils'

export const useSelectAuctions = (pageSize: number, page: number) => {
  return useQuery<PagedResult<Auction>>({
    queryKey: ['auctions', pageSize, page],
    queryFn: async () => {
      const response = await axios.get(
        `${BackendAppUrl}/search?pageNumber=${page}&pageSize=${pageSize}`,
      )

      if (response.status !== 200) {
        throw new Error(`Error fetching auctions: ${response.statusText}`)
      }

      return response.data
    },
  })
}
