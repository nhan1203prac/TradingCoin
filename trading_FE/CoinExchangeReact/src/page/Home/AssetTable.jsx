import React from 'react'
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
import { useNavigate } from 'react-router-dom'
import { ScrollArea } from '@/components/ui/scroll-area'
  
const AssetTable = ({coin,category}) => {
  const navigator = useNavigate()
  console.log("top50",coin)
  return (
    <Table>
      <ScrollArea className={category=="all"?"h-[70vh]":"h-[75vh]"}>

      
  <TableHeader>
    <TableRow>
      <TableHead className="w-[150px]">COIN</TableHead>
      <TableHead>SYMBOL</TableHead>
      <TableHead>VOLUME</TableHead>
      <TableHead>MARKETCAP</TableHead>

      <TableHead>24h</TableHead>

      <TableHead className="text-right">PRICE</TableHead>
    </TableRow>
  </TableHeader>
  <TableBody>
    {coin.map((item,index)=><TableRow key={item.id}>
      <TableCell onClick={()=>navigator(`/market/${item.id}`)} className="font-medium flex items-center gap-2 ">
        <Avatar className='z-50'>
            <AvatarImage src={item.image} className="w-[40px] h-[40px]"/>
        </Avatar>
        <span>{item.name}</span>
      </TableCell>
      <TableCell>{item.symbol}</TableCell>
      <TableCell>{item.totalVolume}</TableCell>
      <TableCell>{item.marketCap}</TableCell>
      <TableCell>{item.priceChangePercentage24h}</TableCell>

      <TableCell className="text-right">${item.currentPrice}</TableCell>
    </TableRow>)
        
    }
    
  </TableBody>
  </ScrollArea>
</Table>

  )
}

export default AssetTable
