import { useQuery } from '@tanstack/react-query'
import axios from 'axios'

export const useSelectAuctions = () => {
  return useQuery({
    queryKey: ['auctions'],
    queryFn: async () => {
      const response = await axios.get('http://localhost:5003/search')

      if (response.status !== 200) {
        throw new Error(`Error fetching auctions: ${response.statusText}`)
      }

      return response.data
    },
  })
}
