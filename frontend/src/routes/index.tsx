import { createFileRoute } from '@tanstack/react-router'

import { Listings } from '@/components/Auctions/Listings'
import { BackendAppUrl } from '@/lib/util'

export const Route = createFileRoute('/')({
  component: App,
})

function App() {
  console.log(BackendAppUrl)
  return (
    <div>
      <Listings />
    </div>
  )
}
