import { useQuery } from '@tanstack/react-query'
import axios from 'axios'
import type { Auction, PagedResult } from '../../types'
import { BackendAppUrl } from '@/lib/util'

export const useSelectAuctions = (pageSize: number) => {
  return useQuery<PagedResult<Auction>>({
    queryKey: ['auctions', pageSize],
    queryFn: async () => {
      const response = await axios.get(
        `${BackendAppUrl}/search?pageSize=${pageSize}`,
      )

      if (response.status !== 200) {
        throw new Error(`Error fetching auctions: ${response.statusText}`)
      }

      return response.data
    },
  })
}
