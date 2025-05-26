import { Badge } from '@/components/ui/badge'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { VerifiedIcon } from 'lucide-react'
import React, { useEffect, useState } from 'react'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import AccountVerificationForm from './AccountVerificationForm'
import { useDispatch, useSelector } from 'react-redux'
import { getUser } from '@/State/Auth/Action'

const Profile = () => {

  const dispatch = useDispatch()
  const auth = useSelector((state) => state.auth)
  const [refresh, setRefresh] = useState(false);
  useEffect(() => {
    const storedJwt = localStorage.getItem("jwt");  
    if (storedJwt) {
      
      dispatch(getUser(storedJwt));
      
    }
  }, []);

  const handleRefresh = () => {
    setRefresh((prev) => !prev); 
  };
  console.log("user", auth)
  return (
    <div className='flex flex-col items-center mb-5'>
        <div className="pt-10 w-full lg:w-[60%]">
            <Card>
                <CardHeader className="pb-9">
                    <CardTitle>Your information</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className='lg:flex gap-32'>
                      <div className="space-y-7">
                          <div className="flex">
                              <p className='w-[9rem]'>Email : </p>
                              <p className='text-gray-500'>{auth.user?.email}</p>
                          </div>
                          <div className="flex">
                              <p className='w-[9rem]'>Full Name : </p>
                              <p className='text-gray-500'>{auth.user?.fullName}</p>
                          </div>
                          <div className="flex">
                              <p className='w-[9rem]'>Date of Birth : </p>
                              <p className='text-gray-500'>12/03/2004</p>
                          </div>
                          <div className="flex">
                              <p className='w-[9rem]'>Nationality : </p>
                              <p className='text-gray-500'>VietNam</p>
                          </div>
                      </div>
                      <div className="space-y-7">
                          <div className="flex">
                              <p className='w-[9rem]'>Address : </p>
                              <p className='text-gray-500'>125/03 Ngo Gia Tu, Hai Chau, Da Nang</p>
                          </div>
                          <div className="flex">
                              <p className='w-[9rem]'>City : </p>
                              <p className='text-gray-500'>Da Nang</p>
                          </div>
                         
                      </div>
                  </div>
                </CardContent>
            </Card>
            <div className="mt-6">
                <Card className="w-full">
                    <CardHeader className="pb-7">
                        <div className="flex items-center gap-3">
                            <CardTitle>2 step verification</CardTitle>
                            {!auth.user.isEnable2FA?
                            <Badge className={"space-x-2 text-white bg-green-600"}>
                            <VerifiedIcon/>
                            <span>Enabled</span>
                            </Badge>:
                          <Badge  className={"bg-orange-500"}>Disabled</Badge>}
                        </div>
                    </CardHeader>
                    <CardContent>
                      <div className="">
                      <Dialog>
                        <DialogTrigger>Enabled Two Step Verification</DialogTrigger>
                        <DialogContent>
                          <DialogHeader>
                            <DialogTitle>Verify your account</DialogTitle>
                            <DialogDescription>
                              This action cannot be undone. This will permanently delete your account
                              and remove your data from our servers.
                            </DialogDescription>
                          </DialogHeader>
                          <AccountVerificationForm onVerificationSuccess={handleRefresh} />
                        </DialogContent>
                      </Dialog>

                      </div>
                    </CardContent>
                </Card>
            </div>
        </div>
    </div>
  )
}

export default Profile
