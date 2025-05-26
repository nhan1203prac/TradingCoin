import {Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import React, { useEffect } from 'react'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Button } from '@/components/ui/button'
import PaymentDetailsForm from './PaymentDetailsForm'
import { useDispatch, useSelector } from 'react-redux'
import { getPaymentDetail } from '@/State/Withdrawal/Action'

const PaymentDetails = () => {

  const withdrawal = useSelector(state=>state.withdrawal)
  const dispatch = useDispatch()

  useEffect(()=>{
    dispatch(getPaymentDetail({jwt:localStorage.getItem("jwt")}))
    
  },[])
  return (
    <div className='px-20'>
        <h1 className='text-3xl font-bold py-10'>Payment Details</h1>
        
        
        <Card className = "mb-5">
            <CardHeader>
                <CardTitle>
                    {withdrawal.paymentDetails?.bankName
                    }
                </CardTitle>
                <CardDescription>
                  A/C No :
                  *********** {withdrawal.paymentDetails?.accountNumber?.substring(withdrawal.paymentDetails.accountNumber.length - 4)}
                </CardDescription>
            </CardHeader>

            <CardContent>
              <div className="flex items-center">
                  <p className='w-32'>A/C Holder</p>
                  <p className='text-gray-400'> : {withdrawal.paymentDetails?.accountHolderName}</p>
              </div>
              <div className="flex items-center">
                  <p className='w-32'>IFSC</p>
                  <p className='text-gray-400'> : {withdrawal.paymentDetails?.ifsc}</p>
              </div>
            </CardContent>
        </Card>
        

            <Dialog>
      <DialogTrigger>
        Add payment details
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Payment Details</DialogTitle>
          
        </DialogHeader>
        <PaymentDetailsForm/>
      </DialogContent>
    </Dialog>

    </div>
  )
}

export default PaymentDetails
