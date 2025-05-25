import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { transferMoney } from '@/State/Wallet/Action'
import { DialogClose } from '@radix-ui/react-dialog'
import React, { useState } from 'react'
import { useDispatch } from 'react-redux'

const TransferForm = () => {
  const dispatch = useDispatch()
  const [formData, setFormData] = useState({amount:"",walletId:"",purpose:""})
  const handleChange = (e)=>{
    setFormData({
      ...formData,
      [e.target.name]:e.target.value
    })
  }

  const handleSubmit = ()=>{
    dispatch(transferMoney({jwt:localStorage.getItem("jwt"),walletId:formData.walletId,reqData:{amount:formData.amount,purpose:formData.purpose}}))
    console.log(formData)
  }
  return (
    <div className='pt-10 space-y-5'>
        <div className="">
            <h2 className='pb-1'>Enter Amount</h2>
            <Input
            name="amount"
            value={formData.amount}
            onChange={handleChange}
            className="py-7"
            placeholder="$9999"
            />
        </div>

        <div className="">
            <h2 className='pb-1'>Wallet Id</h2>
            <Input
            name="walletId"
            value={formData.walletId}
            onChange={handleChange}
            className="py-7"
            placeholder="#ADER455"
            />
        </div>

        <div className="">
            <h2 className='pb-1'>Purpose</h2>
            <Input
            name="purpose"
            value={formData.purpose}
            onChange={handleChange}
            className="py-7"
            placeholder="Gift for your friend"
            />
        </div>
        <DialogClose className='w-full'>
            <Button className="w-full py-7" onClick={handleSubmit}>
                Submit
            </Button>
        </DialogClose>
    </div>
  )
}

export default TransferForm
