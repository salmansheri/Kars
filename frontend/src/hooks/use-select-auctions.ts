
import { useQuery } from '@tanstack/react-query'
import axios from 'axios';
import { BackendAppUrl } from '@/lib/util'

export const useSelectAuctions = (pageSize: number) => {
  return useQuery({
    queryKey: ['auctions'],
    queryFn: async () => {
      const response = await axios.get(`${BackendAppUrl}/search?pageSize=${pageSize}`)

      if (response.status !== 200) {
        throw new Error(`Error fetching auctions: ${response.statusText}`)
      }

      return response.data
    },
  })
}
