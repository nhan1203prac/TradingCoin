import { DialogClose } from '@radix-ui/react-dialog'
import React, { useState } from 'react'
import { ToastContainer, toast } from 'react-toastify'  
import 'react-toastify/dist/ReactToastify.css';
import {
    InputOTP,
    InputOTPGroup,
    InputOTPSeparator,
    InputOTPSlot,
} from "@/components/ui/input-otp"
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import axios from 'axios' // Make sure axios is imported
import { useSelector } from 'react-redux';

const AccountVerificationForm = ({ onVerificationSuccess }) => {
    const [value, setValue] = useState('')
    const [sessionId, setSessionId] = useState(null);
    const auth = useSelector((state) => state.auth)
  
    const jwt = localStorage.getItem("jwt"); // Assuming jwt is stored in localStorage

    const handleSubmit = async () => {
        if (!sessionId) {
            console.error("Session ID is not set. Please send OTP first.");
            return;
        }

        const baseUrl = "http://localhost:8080/api";
        try {
            const response = await axios.post(
                `${baseUrl}/user/enable-two-factor/verify/${value}?id=${sessionId}`,
                null);

                if (onVerificationSuccess) {
                    onVerificationSuccess(); 
                  }    
            console.log("OTP verified successfully:", response.data);
            // toast.success("OTP verified successfully!", { position: toast.POSITION.TOP_RIGHT });
        } catch (error) {
            console.error("Failed to verify OTP:", error.response?.data || error.message);
            // toast.error("Failed to verify OTP", { position: toast.POSITION.TOP_RIGHT });
        }
    };

    const sendOtp = async () => {
        const baseUrl = "http://localhost:8080/api"
        try {
            const response = await axios.post(`${baseUrl}/user/enable-two-factor/send-otp`, null, {
                headers: {
                    Authorization: `Bearer ${jwt}`
                }
            })
            setSessionId(response.data.session);
            // console.log(response.data) // You can log the response if needed
        } catch (error) {
            console.log(error)
        }
    }

    return (
        <div className='flex justify-center'>
            <div className="space-y-5 mt-10 w-full">
                <div className="flex justify-between items-center">
                    <p>Email :</p>
                    <p>{auth.user?.email}</p>
                    <Dialog>
                        <DialogTrigger onClick={sendOtp}>Send OTP</DialogTrigger>
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
                                <DialogClose className='w-[10rem]' onClick={handleSubmit}>
                                    Submit
                                </DialogClose>
                            </div>
                        </DialogContent>
                    </Dialog>
                </div>
            </div>
            {/* Add ToastContainer to display toast notifications */}
            <ToastContainer />
        </div>
    )
}

export default AccountVerificationForm
