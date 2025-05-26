import React, { useEffect } from 'react'
import {
    Table,
    TableBody,
    TableCaption,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
  } from "@/components/ui/table"
import { Avatar } from '@radix-ui/react-avatar'
import { AvatarImage } from '@/components/ui/avatar'
import { Bookmark } from 'lucide-react'
import { BookmarkFilledIcon } from '@radix-ui/react-icons'
import { Button } from '@/components/ui/button'
import { useDispatch, useSelector } from 'react-redux'
import { addItemToWatchlist, getUserWatchlist } from '@/State/Watchlist/Action'

const WatchList = () => {
    const {watchlist} = useSelector(store=>store)
    const dispatch = useDispatch()
   

    useEffect(()=>{
      dispatch(getUserWatchlist({jwt:localStorage.getItem("jwt")}))
    },[])

    const handleRemoveToWatchlist = (item)=>{
     
      // console.log("exits", ExitCoinInWatchlist(watchlist.items,coin.coinDetails))
      dispatch(addItemToWatchlist({coinId:item.id,jwt:localStorage.getItem("jwt")}))
    }
  return (
    <div className="p-5 lg:px-20">
        <h1 className='font-bold text-3xl pb-5'>Watchlist</h1>
        <Table className="border">
  <TableHeader>
    <TableRow>
      <TableHead >COIN</TableHead>
      <TableHead>SYMBOL</TableHead>
      <TableHead>VOLUME</TableHead>
      <TableHead>MARKETCAP</TableHead>

      <TableHead>24h</TableHead>

      <TableHead className="">PRICE</TableHead>
      <TableHead className="text-right text-red-600">REMOVE</TableHead>
    </TableRow>
  </TableHeader>
  <TableBody>
    {watchlist.items.map((item,index)=><TableRow key={index}>
      <TableCell className="font-medium flex items-center gap-2">
        <Avatar className='z-50'>
            <AvatarImage src={item.image} className="w-[35px]"/>
        </Avatar>
        <span>{item.name}</span>
      </TableCell>
      <TableCell>{item.symbol.toUpperCase()}</TableCell>
      <TableCell>{item.totalVolume}</TableCell>
      <TableCell>{item.marketCap}</TableCell>
      <TableCell>{item.priceChangePercentage24h}</TableCell>

      <TableCell className="">${item.currentPrice}</TableCell>
      <TableCell className="text-right">
        <Button variant="outline" size="icon" className="text-white" onClick={()=>handleRemoveToWatchlist(item)}>
            <BookmarkFilledIcon/>
        </Button>
      </TableCell>
    </TableRow>)
        
    }
    
  </TableBody>
</Table>
  </div>
  )
}

export default WatchList
