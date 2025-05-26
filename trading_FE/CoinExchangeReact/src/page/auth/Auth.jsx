import "./auth.css"
import React from 'react'
import SignupForm from "./SignupForm"
import { Button } from "@/components/ui/button"
import { useLocation, useNavigate } from "react-router-dom"
import ForgotPasswordForm from "./ForgotPasswordForm"
import SigninForm from "./SigninForm"
import UpdatePassword from "./UpdatePassword"
const Auth = () => {
  const navigate = useNavigate();
  const localtion = useLocation();
  return (
    <div className='h-screen  authContainer  '>
        <div className="absolute top-0 right-0 left-0 bottom-0 bg-[#030712] bg-opacity-50" >
            <div className="  absolute  top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 flex flex-col justify-center
            items-center  h-[35rem] w-[30rem] rounded-md z-50 bg-black bg-opacity-50 shadow-2xl ">
              <h1 className="text-6xl font-bold pb-9">Trading Cypto</h1>
              {localtion.pathname=="/signup"?(
                <section className="w-full">
                <SignupForm/>
                <div className="flex items-center justify-center">
                    <span>Have already account?</span>
                    <Button variant="ghost" onClick={()=>navigate("/signin")} className="bg-transparent outline-none text-white">Signin</Button>
                </div>
            </section>
              ): location.pathname=="/forgot-password"?(
                <section className="w-full">
                  <ForgotPasswordForm/>
                  <div className="flex items-center justify-center">
                    <span>Back to login</span>
                    <Button variant="ghost" onClick={()=>navigate("/signin")} className="bg-transparent outline-none text-white">Signin</Button>
                </div>
                </section>
              ):localtion.pathname == "/update-password"?
              (
                <section  className="w-full">
                  <UpdatePassword/>
                  <div className="flex items-center justify-center">
                    <span>SignIn</span>
                    <Button variant="ghost" onClick={()=>navigate("/signin")} className="bg-transparent outline-none text-white" >Signup</Button>
                </div>

                
                </section>
              ):(
                <section  className="w-full">
                  <SigninForm/>
                  <div className="flex items-center justify-center">
                    <span>Don't have account?</span>
                    <Button variant="ghost" onClick={()=>navigate("/signup")} className="bg-transparent outline-none text-white" >Signup</Button>
                </div>

                <div className="px-10 my-2">
                    
                    <Button className="w-full py-5 text-white" onClick={()=>navigate("/forgot-password")} variant="outline">Forgot Password</Button>
                </div>
                </section>
              )}
            </div>
        </div>
    </div>
  )
}

export default Auth
