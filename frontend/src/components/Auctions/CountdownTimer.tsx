import Countdown, { zeroPad } from 'react-countdown'

interface CountdownProps {
  auctionEnd: string
}

const renderer = ({
  days,
  hours,
  minutes,
  seconds,
  completed,
}: {
  days: number
  hours: number
  minutes: number
  seconds: number
  completed: boolean
}) => {

  return (
    <div className={`border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center ${completed ? 'bg-red-600': (days === 0 && hours < 10)? 'bg-amber-600': 'bg-green-600'}`}>
      {completed ? (
        <span>Auction Finished</span>
      ): (
        <span> {days}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}</span>
      )}
    </div>
  )
  // if (completed) {
  //   // Render a completed state
  //   return <Completionist />
  // } else {
  //   // Render a countdown
  //   return (
  //     <span>
  //       {days}:{hours}:{minutes}:{seconds}
  //     </span>
  //   )
  // }
}

// const Completionist = () => <span>You are good to go</span>

const CountdownTimer = ({ auctionEnd }: CountdownProps) => {
  return <Countdown date={auctionEnd} renderer={renderer} />
}

export default CountdownTimer
