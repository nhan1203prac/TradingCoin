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
import { getWithdrawalHistory } from '@/State/Withdrawal/Action'
const Withdrawal = () => {
  const dispatch = useDispatch()
  const {wallet,withdrawal} = useSelector(store=>store)

  useEffect(()=>{
    dispatch(getWithdrawalHistory({jwt:localStorage.getItem("jwt")}))
  },[])
  return (
    <div className="p-5 lg:px-20">
        <h1 className='font-bold text-3xl pb-5'>Activity</h1>
        <Table className="border">
  <TableHeader>
    <TableRow>
      <TableHead className="py-5">Date</TableHead>
      <TableHead>Method</TableHead>
      <TableHead>Amount</TableHead>
      
      <TableHead className="text-right">Status</TableHead>
    </TableRow>
  </TableHeader>
  <TableBody>
    {withdrawal.history.map((item,index)=><TableRow key={index}>
      <TableCell>
        <p>{item.date.toString()}</p>
       
      </TableCell>
      
      <TableCell>Bank</TableCell>
     

      <TableCell className="">${item.amount}</TableCell>
      <TableCell className="text-right">
        {item.status==0?"PENDING":item.status==1?"SUCCESS":"DECLINE"}
      </TableCell>
    </TableRow>)
        
    }
    
  </TableBody>
</Table>
  </div>
  )
}

export default Withdrawal
