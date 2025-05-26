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
import { login } from '@/State/Auth/Action'
import { useLocation, useNavigate } from 'react-router-dom'
import { toast,ToastContainer } from "react-toastify";
import axios from 'axios'
const UpdatePassword = () => {
    const form = useForm({
        resolver:"",
        defaultValues:{
            
          password:"",
          confirmPassword:"",
           
        }
    })
    const location = useLocation();
    const email = location.state?.email;
    const navigate = useNavigate()
    const dispatch = useDispatch()
  

    const onSubmit = async (data) => {
      const baseUrl = "http://localhost:8080/api";
      try {
        console.log("data", email); 
        const response = await axios.patch(
          `${baseUrl}/user/update-password`,
          { password:data.password, confirmPassword:data.confirmPassword, email:email },
          {
            headers: {
              "Content-Type": "application/json", 
            },
          }
        );
    
        
        console.log("update password success:", response.data);
        navigate("/signin")
        toast.success("Update password successfully!");
      } catch (error) {
        console.error("Failed to update:", error.response?.data || error.message);
        toast.error("Failed to update");
      }
    };
  return (
    <div className='px-10 max-h-[90vh]' >
        <h1 className='text-xl font-bold text-center pb-3'>Update Password</h1>
        <ToastContainer position="top-right" />
        <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
           

<FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input name="password" placeholder="Password" {...field} className="border w-full border-gray-700 p-5"/>
              </FormControl>
              
              <FormMessage />
            </FormItem>
          )}
        />

<FormField
          control={form.control}
          name="confirmPassword"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input placeholder="ConfirmPassword" {...field} className="border w-full border-gray-700 p-5"/>
              </FormControl>
              
              <FormMessage />
            </FormItem>
          )}
        />





        <Button type="submit" className="w-full py-5">Submit</Button>
        
            </form>
        </Form>
    </div>
  )
}


export default UpdatePassword
