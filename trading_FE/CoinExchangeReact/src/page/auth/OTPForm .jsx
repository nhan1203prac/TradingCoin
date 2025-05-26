// OTPForm.js
import React from 'react'
import { Button } from '@/components/ui/button'
import {
  InputOTP,
  InputOTPGroup,
  InputOTPSeparator,
  InputOTPSlot,
} from "@/components/ui/input-otp"
import { useState } from 'react'

const OTPForm = ({ onSubmit }) => {
  const [otp, setOtp] = useState('');

  const handleSubmit = () => {
    onSubmit(otp);
  };

  return (
    <div className="py-5 flex gap-10 justify-center items-center">
      <InputOTP maxLength={6} value={otp} onChange={(value) => setOtp(value)}>
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
      <Button className="w-[10rem]" onClick={handleSubmit}>
        Gửi
      </Button>
    </div>
  );
};

export default OTPForm;
