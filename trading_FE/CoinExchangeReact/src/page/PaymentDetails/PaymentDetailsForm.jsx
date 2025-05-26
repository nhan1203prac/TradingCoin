
import React from 'react'
import { useForm } from 'react-hook-form'
import {
    Form,
    FormControl,
    FormDescription,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
  } from "@/components/ui/form"
  import { Input } from "@/components/ui/input"
import { Button } from '@/components/ui/button'
import { DialogClose } from '@/components/ui/dialog'
import { useDispatch } from 'react-redux'
import { addPaymentDetails } from '@/State/Withdrawal/Action'
  
const PaymentDetailsForm = () => {
  const dispatch = useDispatch()

    const form = useForm({
        resolver:"",
        defaultValues:{
            accountHolderName:"",
            ifsc:"",
            accountNumber:"",
            bankName:""
        }
    })
    const onSubmit = (data)=>{
        dispatch(addPaymentDetails({paymentDetail:data,jwt:localStorage.getItem("jwt")}))
        console.log(data)
    }

  return (
    <div className='px-10 max-h-[90vh]' >
        <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
            <FormField
          control={form.control}
          name="accountHolderName"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Account Holder Name</FormLabel>
              <FormControl>
                <Input name="accountHolderName" placeholder="Nguyen Van A" {...field} className="border w-full border-gray-700 p-5"/>
              </FormControl>
              
              <FormMessage />
            </FormItem>
          )}
        />

<FormField
          control={form.control}
          name="ifsc"
          render={({ field }) => (
            <FormItem>
              <FormLabel>IFSC Code</FormLabel>
              <FormControl>
                <Input name="ifsc" placeholder="Nguyen Van A" {...field} className="border w-full border-gray-700 p-5"/>
              </FormControl>
              
              <FormMessage />
            </FormItem>
          )}
        />

<FormField
          control={form.control}
          name="accountNumber"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Account Number</FormLabel>
              <FormControl>
                <Input placeholder="***********5604" {...field} className="border w-full border-gray-700 p-5"/>
              </FormControl>
              
              <FormMessage />
            </FormItem>
          )}
        />

<FormField
          control={form.control}
          name="confirmAccountNumber"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Confirm Account Number</FormLabel>
              <FormControl>
                <Input placeholder="Confirm account number" {...field} className="border w-full border-gray-700 p-5"/>
              </FormControl>
              
              <FormMessage />
            </FormItem>
          )}
        />


<FormField
          control={form.control}
          name="bankName"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Bank Name</FormLabel>
              <FormControl>
                <Input placeholder="ViettinBank" {...field} className="border w-full border-gray-700 p-5"/>
              </FormControl>
              
              <FormMessage />
            </FormItem>
          )}
        />
        <DialogClose className='w-full' type="submit">
            Submit
        </DialogClose>
        
            </form>
        </Form>
    </div>
  )
}

export default PaymentDetailsForm
