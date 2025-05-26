import React, { useEffect, useState } from 'react'
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
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
    DialogClose
  } from "@/components/ui/dialog"
  import { Input } from "@/components/ui/input"
import { Button } from '@/components/ui/button'
import {
  InputOTP,
  InputOTPGroup,
  InputOTPSeparator,
  InputOTPSlot,
} from "@/components/ui/input-otp"
import { useDispatch } from 'react-redux'
import { login } from '@/State/Auth/Action'
import { useNavigate } from 'react-router-dom'
import { toast,ToastContainer } from "react-toastify";
import axios from 'axios'

const SigninForm = () => {
    const form = useForm({
        resolver:"",
        defaultValues:{
            
            email:"",
            password:"",
           
        }
    })
    const [showOtpDialog, setShowOtpDialog] = useState(false);
    const [session, setSession] = useState(null);
    const [otp, setOtp] = useState('');
    const navigate = useNavigate()
    const dispatch = useDispatch()
    const [jwt ,setJwt] = useState()

    const onSubmit = async(data)=>{
      try {
        const response = await dispatch(login({ data, navigate }));
        setJwt(response.jwt)
        console.log("response",response.isTwoFatorAuthEnabled)
        if (response.isTwoFatorAuthEnabled) {
          setShowOtpDialog(true);
          sendOtp(data.email)  
        }
        else{
          navigate("/");
        }
      } catch (error) {
        console.error("Login error:", error);
      }
        
    }
    const sendOtp = async (email) => {
      const baseUrl = "http://localhost:8080/auth";
      try {
        console.log("email", email); 
       
        const response = await axios.post(
          `${baseUrl}/reset-password/send-otp`,
          { sendTo:email },
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
    const verifyOtp = async()=>{
      const baseUrl = "http://localhost:8080/auth";
          try {
              const response = await axios.post(
                  `${baseUrl}/reset-password/verify-otp/${value}?id=${session}`,
                  null);
  
              setShowOtpDialog(false)
              console.log("verify otp success:", response.data);
              localStorage.setItem("jwt", jwt);
              window.location.reload();    
              // navigate("/")
              // toast.success("OTP verified successfully!", { position: toast.POSITION.TOP_RIGHT });
          } catch (error) {
              console.error("Failed to send OTP:", error.response?.data || error.message);
              // toast.error("Failed to verify OTP", { position: toast.POSITION.TOP_RIGHT });
          }
     
    }
    useEffect(() => {
      if (showOtpDialog) {
        console.log("Hộp thoại OTP sẽ hiển thị.");
      }
    }, [showOtpDialog]);
    const [value, setValue] = useState('')
    return (
      <div className="px-10 max-h-[90vh]">
        <h1 className="text-xl font-bold text-center pb-3">Sign in</h1>
        <ToastContainer position="top-right" />
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormControl>
                    <Input name="email" placeholder="Email" {...field} className="border w-full border-gray-700 p-5" />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
  
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormControl>
                    <Input name="password" placeholder="Password" {...field} className="border w-full border-gray-700 p-5" />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
  
            <Button type="submit" className="w-full py-5">Submit</Button>
          </form>
        </Form>
  
        {/* OTP Dialog */}
        {showOtpDialog && (
          <Dialog open={showOtpDialog} onOpenChange={setShowOtpDialog}>
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
                <Button className="w-[10rem]" onClick={verifyOtp}>
                  Submit
                </Button>
              </div>
            </DialogContent>
          </Dialog>
        )}
      </div>
    );
}


export default SigninForm
