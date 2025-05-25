
import React, { useEffect } from 'react'
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
import { z } from 'zod'
import { zodResolver } from "@hookform/resolvers/zod";
  
const formSchema = z.object({
  accountHolderName: z.string().min(2, { message: "Account holder name must be at least 2 characters." }),
  ifsc: z.string().min(3, { message: "IFSC code is required." }), // Thêm validation nếu cần
  bankName: z.string().min(2, { message: "Bank name is required." }),
  accountNumber: z.string().min(5, { message: "Account number is required." }),
  // Thêm confirmAccountNumber nếu bạn muốn validate nó
  confirmAccountNumber: z.string()
}).refine((data) => data.accountNumber === data.confirmAccountNumber, {
  message: "Account numbers don't match",
  path: ["confirmAccountNumber"], // Báo lỗi ở trường confirm
});
const PaymentDetailsForm = ({existingDetails}) => {
  const dispatch = useDispatch()

    const form = useForm({
        resolver:zodResolver(formSchema),
        defaultValues:{
            accountHolderName:"",
            ifsc:"",
            accountNumber:"",
            confirmAccountNumber: "",
            bankName:""
        }
    })

    useEffect(()=>{
      if (existingDetails) {
      form.reset({
        accountHolderName: existingDetails.accountHolderName,
        ifsc: existingDetails.ifsc,
        accountNumber: existingDetails.accountNumber,
        bankName: existingDetails.bankName,
        confirmAccountNumber: existingDetails.accountNumber // Tự điền confirm khi edit
      });
    } else {
      // Nếu không có (chế độ Add), reset về rỗng
      form.reset({
        accountHolderName: "",
        ifsc: "",
        accountNumber: "",
        confirmAccountNumber: "",
        bankName: ""
      });
    }
    },[existingDetails])
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
          <DialogClose asChild>
            <Button type="submit" className='w-full'>
                Submit
            </Button>
          </DialogClose>
        
            </form>
        </Form>
    </div>
  )
}

export default PaymentDetailsForm
