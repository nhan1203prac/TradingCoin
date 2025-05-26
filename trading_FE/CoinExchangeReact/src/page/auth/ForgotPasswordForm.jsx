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

const ForgotPasswordForm = () => {
    const form = useForm({
        resolver:"",
        defaultValues:{
            
            email:"",
            
           
        }
    })
    const onSubmit = (data)=>{
        console.log(data)
    }

  return (
    <div className='px-10 max-h-[90vh]' >
        <h1 className='text-xl font-bold text-center pb-3'>Forgot password</h1>

        <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className='space-y-6'>
           

<FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input name="email" placeholder="Email" {...field} className="border w-full border-gray-700 p-5"/>
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

export default ForgotPasswordForm
