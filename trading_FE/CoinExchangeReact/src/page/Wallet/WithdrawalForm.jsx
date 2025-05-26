import React, { useEffect, useState } from 'react'
import { Input } from '@/components/ui/input'
import { Button } from '@/components/ui/button'
import { DialogClose } from '@radix-ui/react-dialog'
import { useDispatch, useSelector } from 'react-redux'
import { getPaymentDetail, withdrawalRequest } from '@/State/Withdrawal/Action'
const WithdrawalForm = () => {
  const [amount, setAmount] = useState("")
  const dispatch = useDispatch()
  const {wallet,withdrawal} = useSelector(store=>store)
  
 
  useEffect(()=>{
    dispatch(getPaymentDetail({jwt:localStorage.getItem("jwt")}))
    
  },[])
  const handleChange = (e)=>{
    setAmount(e.target.value)
  }
 const handleSubmit = ()=>{
  dispatch(withdrawalRequest({amount,jwt:localStorage.getItem("jwt")}))
  // console.log(amount)
 }
  return (
    <div className='pt-10 space-y-5'>
      <div className="flex justify-between items-center rounded-md bg-slate-900 text-xl font-bold px-5 py-4">
        <p>Available balance</p>
        <p>${wallet.userWallet.balance}</p>
      </div>
      <div className="flex flex-col items-center">
          <h3>Enter Withdrawal amount</h3>
          <div className="flex items-center justify-center">
            <Input onChange={handleChange} value={amount} className="withdrawalInput py-7 border-none outline-none
            focus:outline-none px-0 text-2xl text-center" placeholder="$999" type="number"/>
          </div>
      </div>
      <div className="">
          <p className='pb-2'>Transfer to</p>
          <div className="flex items-center gap-5  border px-5 py-2 rounded-md">
              <img className='h-8 w-8' src="data:image/jpeg;base64,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" alt="" />
              <div className="">
                  <p className='text-xl font-bold'>ViettinBank</p>
                  <p className='text-xs'>***********{withdrawal.paymentDetails?.accountNumber?.substring(withdrawal.paymentDetails.accountNumber.length - 4)}</p>
              </div>
          </div>
      </div>
      <DialogClose className='w-full'>
        <Button onClick={handleSubmit} className="w-full py-7 text-xl">Withdrawal</Button>
      </DialogClose>
    </div>
  )
}

export default WithdrawalForm
