import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
  DialogClose
} from "@/components/ui/dialog"
import AccountVerificationForm from '../Profile/AccountVerificationForm'
import { Input } from "@/components/ui/input"
import { Button } from '@/components/ui/button'
import {
  InputOTP,
  InputOTPGroup,
  InputOTPSeparator,
  InputOTPSlot,
} from "@/components/ui/input-otp"
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import { toast,ToastContainer } from "react-toastify";
const ForgotPasswordForm = () => {
  const form = useForm({
    resolver: "",
    defaultValues: {
      sendTo: "",
    }
  })
  const [session,setSession] = useState(null)
  const [sendTo,setSendTo] = useState()
  const navigate = useNavigate()
  const onSubmit = async (data) => {
    const baseUrl = "http://localhost:8080/auth";
    try {
      console.log("sendTo", data.sendTo); 
      setSendTo(data.sendTo)
      const response = await axios.post(
        `${baseUrl}/reset-password/send-otp`,
        { sendTo:data.sendTo },
        {
          headers: {
            "Content-Type": "application/json", // Xác định loại dữ liệu gửi đi
          },
        }
      );
  
      setSession(response.data.session);
      console.log("send otp success:", response.data);
      // toast.success("OTP sent successfully!", { position: toast.POSITION.TOP_RIGHT });
    } catch (error) {
      console.error("Failed to send OTP:", error.response?.data || error.message);
      toast.error("Failed to verify OTP");
    }
  };
  
  const sendOtp = async()=>{
    const baseUrl = "http://localhost:8080/auth";
        try {
            const response = await axios.post(
                `${baseUrl}/reset-password/verify-otp/${value}?id=${session}`,
                null);

            
            console.log("verify otp success:", response.data);
            navigate("/update-password",{ state: { email: sendTo } })
            // toast.success("OTP verified successfully!", { position: toast.POSITION.TOP_RIGHT });
        } catch (error) {
            console.error("Failed to send OTP:", error.response?.data || error.message);
            // toast.error("Failed to verify OTP", { position: toast.POSITION.TOP_RIGHT });
        }
   
  }
  const [value, setValue] = useState('')

  return (
    <div className='px-10 max-h-[90vh]'>
      <h1 className='text-xl font-bold text-center pb-3'>Forgot password</h1>
      <ToastContainer position="top-right" />
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
          <FormField
            control={form.control}
            name="sendTo"
            render={({ field }) => (
              <FormItem>
                <FormControl>
                  <Input name="sendTo"   placeholder="Email" {...field} className="border w-full border-gray-700 p-5" />
                </FormControl>

                <FormMessage />
              </FormItem>
            )}
          />

          {/* Button inside Dialog */}
          <Dialog>
            <DialogTrigger asChild>
              <Button type="submit" className="w-full py-5">
                Submit
              </Button>
            </DialogTrigger>
            <DialogContent>
                            <DialogHeader>
                                <DialogTitle>Enter OTP</DialogTitle>
                            </DialogHeader>
                            <div className="py-5 flex gap-10 justify-center items-center">
                                <InputOTP maxLength={6} value={value} onChange={(value) => setValue(value)}>
                                    <InputOTPGroup>
                                        <InputOTPSlot index={0} />
                                        <InputOTPSlot index={1} />
                                        <InputOTPSlot index={2} />
                                    </InputOTPGroup>
                                    <InputOTPSeparator />
                                    <InputOTPGroup>
                                        <InputOTPSlot index={3} />
                                        <InputOTPSlot index={4} />
                                        <InputOTPSlot index={5} />
                                    </InputOTPGroup>
                                </InputOTP>
                                <Button className='w-[10rem]' onClick={sendOtp}>
                                    Submit
                                </Button  >
                            </div>
                        </DialogContent>
          </Dialog>
        </form>
      </Form>
    </div>
  )
}

export default ForgotPasswordForm
