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
import { getAllOrderForUser } from '@/State/Order/Action'
import { CalculateProfile } from '@/until/CalculateProfile'
const Activity = () => {
  const dispatch = useDispatch()
  const {order} = useSelector(store=>store)

  useEffect(()=>{
    dispatch(getAllOrderForUser({jwt:localStorage.getItem("jwt")}))
  },[])
  return (
    <div className="p-5 lg:px-20">
        <h1 className='font-bold text-3xl pb-5'>Activity</h1>
        <Table className="border">
  <TableHeader>
    <TableRow>
      <TableHead className="py-5">Date & Time</TableHead>
      <TableHead>Treading pair</TableHead>
      <TableHead>Buy Price</TableHead>
      <TableHead>Sell Price</TableHead>

      <TableHead>Order Type</TableHead>
      
      <TableHead className="">Profite/Loss</TableHead>
      <TableHead className="text-right">Value</TableHead>
    </TableRow>
  </TableHeader>
  <TableBody>
    {order.orders.map((item,index)=><TableRow key={index}>
      <TableCell>
        <p>{item.timestamps.substring(0,10)}</p>
        <p className='text-gray-400'>{item.timestamps.substring(15,19)}</p>
      </TableCell>
      <TableCell className="font-medium flex items-center gap-2">
        <Avatar className='z-50'>
            <AvatarImage src={item.coin.image} className="w-[35px]"/>
        </Avatar>
        <span>{item.coin.name}</span>
      </TableCell>
      <TableCell>${item.buyPrice}</TableCell>
      <TableCell>${item.sellPrice}</TableCell>
      <TableCell>{item.orderType==1?"BUY":"SELL"}</TableCell>

      <TableCell className="">{CalculateProfile(item)}</TableCell>
      <TableCell className="text-right">
        ${item.price}
      </TableCell>
    </TableRow>)
        
    }
    
  </TableBody>
</Table>
  </div>
  )
}

export default Activity
