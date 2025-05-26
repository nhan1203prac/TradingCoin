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
import { useDispatch, useSelector } from 'react-redux'
import { getUserAssets } from '@/State/Asset/Action'
const Portfolio = () => {
  const dispatch = useDispatch()
  const asset = useSelector(state=>state.asset)
  useEffect(()=>{
    dispatch(getUserAssets({jwt:localStorage.getItem("jwt")}))
  },[])
  return (
    <div className="p-5 lg:px-20">
        <h1 className='font-bold text-3xl pb-5'>Portfolio</h1>
    <Table>
    <TableHeader>
      <TableRow>
        <TableHead className="">ASSETS</TableHead>
        <TableHead >SYMBOL</TableHead>
        <TableHead>UNIT</TableHead>
        <TableHead>CHANGE</TableHead>
  
        <TableHead>CHANGE%</TableHead>
  
        <TableHead className="text-right">VOLUME</TableHead>
      </TableRow>
    </TableHeader>
    <TableBody>
      {asset.userAssets.map((item,index)=><TableRow key={index}>
        <TableCell className="font-medium flex items-center gap-2">
          <Avatar className='z-50'>
              <AvatarImage src={item.coin.image} className="w-[30px]"/>
          </Avatar>
          <span>{item.coin.name}</span>
        </TableCell>
        <TableCell>{item.coin.symbol.toUpperCase()}</TableCell>
        <TableCell>{item.quantity}</TableCell>
        <TableCell>{item.coin.priceChange24h}</TableCell>
        <TableCell>
          {item.coin.priceChangePercentage24h}%
        </TableCell>
  
        <TableCell className="text-right">{item.coin.totalVolume}</TableCell>
      </TableRow>)
          
      }
      
    </TableBody>
  </Table>
  </div>
  )
}

export default Portfolio
