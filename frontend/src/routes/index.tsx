import { createFileRoute } from '@tanstack/react-router'

import { Listings } from '@/components/Auctions/Listings'

export const Route = createFileRoute('/')({
  component: App,
})

function App() {

  return (
    <div>
      <Listings />
    </div>
  )
}
