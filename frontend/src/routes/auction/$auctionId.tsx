import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/auction/$auctionId')({
  component: AuctionIdRoute,
})

function AuctionIdRoute() {
  return <div>Hello</div>
}
