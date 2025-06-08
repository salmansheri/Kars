import Countdown from 'react-countdown'

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
  if (completed) {
    // Render a completed state
    return <Completionist />
  } else {
    // Render a countdown
    return (
      <span>
        {days}:{hours}:{minutes}:{seconds}
      </span>
    )
  }
}

const Completionist = () => <span>You are good to go</span>

const CountdownTimer = ({ auctionEnd }: CountdownProps) => {
  return <Countdown date={auctionEnd} renderer={renderer} />
}

export default CountdownTimer
