import { Link } from "@tanstack/react-router";

interface AuctionCardProps {
    auction: any; 
    isLoading?: boolean; 
}



const AuctionCard = ({auction, isLoading}: AuctionCardProps) => {
    console.log("id: ", auction.id)
  return (
    <>
    
    <Link to="/" className="p-2 rounded-md bg-gray-200/20 backdrop-blur-md group">
        <div className="relative w-full bg-gray-200 aspect-video rounded-lg overflow-hidden">
            <img src={auction.imageUrl} alt={auction.imageUrl} className="object-cover w-full h-full group-hover:scale-110 transition-transform duration-300 ease-in-out" />
        

           
        </div>
        <div className="flex justify-between items-center mt-4">
            <h3 className="text-gray-">{auction.make}</h3>
            <p className="font-semibold text-sm">{auction.year}</p>

        </div>
    </Link>
    </>
  )
}

export default AuctionCard