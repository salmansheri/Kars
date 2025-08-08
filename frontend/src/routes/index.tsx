import { createFileRoute } from '@tanstack/react-router'
import { Listings } from '@/components/Auctions/Listings'

export const Route = createFileRoute('/')({
  component: App,
  validateSearch: (search) => ({page: search.page ? Number(search.page): 1}),
})

function App() {
  const { page } = Route.useSearch();


  return (
    <div>
      <Listings page={page} />
    </div>
  )
}
