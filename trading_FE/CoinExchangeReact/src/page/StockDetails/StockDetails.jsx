import { Avatar, AvatarImage } from '@/components/ui/avatar'
import { Button } from '@/components/ui/button'
import { BookmarkFilledIcon, DotIcon } from '@radix-ui/react-icons'
import { BookMarkedIcon } from 'lucide-react'
import React, { useEffect, useState } from 'react'
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
  } from "@/components/ui/dialog"
import TreadingForm from './TreadingForm'
import StockChart from '../Home/StockChart'
import { useDispatch, useSelector } from 'react-redux'
import { useParams } from 'react-router-dom'
import { fetchCoinDetail } from '@/State/Coin/Action'
import { ExitCoinInWatchlist } from '@/until/ExitsCoinInWatchlist'
import { addItemToWatchlist, getUserWatchlist } from '@/State/Watchlist/Action'
  
const StockDetails = () => {
  const dispatch = useDispatch();
  const { id } = useParams();
  const { coin, watchlist } = useSelector(store => store);
  
  const [data, setData] = useState(null);

  useEffect(() => {
    const existingCoin = coin.top50?.find(item => item.id === id) || coin.coinList?.find(item => item.id === id);
    if (existingCoin) {
  console.log("existingCoin", existingCoin)

      setData(existingCoin);
    } else {
      dispatch(fetchCoinDetail(id));
    }
  }, [id, dispatch]);


  useEffect(() => {
    dispatch(getUserWatchlist({ jwt: localStorage.getItem("jwt") }));
  }, [dispatch]);

  const coinInWatchlist = ExitCoinInWatchlist(watchlist.items || [], data || coin.coinDetails);

  const handleAddToWatchlist = () => {
    dispatch(addItemToWatchlist({ coinId: data?.id || coin.coinDetails?.id, jwt: localStorage.getItem("jwt") }));
  };
  console.log("data", data)
  return (
    <div className='p-5 mt-5'>
      <div className="flex justify-between">
        <div className="flex gap-5 items-center">
          <Avatar>
            <AvatarImage src={data?.image || coin.coinDetails?.image || ''} />
          </Avatar>
          <div>
            <div className="flex items-center gap-2">
              <p>{data?.symbol?.toUpperCase() || coin.coinDetails?.symbol?.toUpperCase()}</p>
              <DotIcon className='text-gray-400' />
              <p className='text-gray-400'>{data?.name || coin.coinDetails?.name}</p>
            </div>
            <div className="flex items-center gap-2">
              <p className='text-xl font-bold'>${data?.current_price || coin.coinDetails?.current_price}</p>
              <span className='text-red-600'>
                <span>{data?.market_cap_change_24h || coin.coinDetails?.market_cap_change_24h}</span>
                <span>({data?.market_cap_change_percentage_24h || coin.coinDetails?.market_data?.market_cap_change_percentage_24h}%)</span>
              </span>
            </div>
          </div>
        </div>
        <div className='flex items-center gap-4'>
          <Button onClick={handleAddToWatchlist}>
            {coinInWatchlist ? <BookmarkFilledIcon className='h-6 w-6' /> : <BookMarkedIcon className='h-6 w-6' />}
          </Button>
          <Dialog>
            <DialogTrigger className='w-[7rem]'>Trade</DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>How much do you want to spend?</DialogTitle>
              </DialogHeader>
              <TreadingForm  data={data}/>
            </DialogContent>
          </Dialog>
        </div>
      </div>
      <div className='mt-10'>
        <StockChart coinId={id} />
      </div>
    </div>
  );
};

export default StockDetails;

