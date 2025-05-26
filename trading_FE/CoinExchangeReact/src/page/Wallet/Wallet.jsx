import React, { useEffect } from 'react'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { DollarSign, WalletIcon } from 'lucide-react'
import { CopyIcon, ReloadIcon, ShuffleIcon, UpdateIcon, UploadIcon } from '@radix-ui/react-icons'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import ToupForm from './ToupForm'
import WithdrawalForm from './WithdrawalForm'
import TransferForm from './TransferForm'
import { Avatar } from '@/components/ui/avatar'
import { AvatarFallback } from '@radix-ui/react-avatar'
import { useDispatch, useSelector } from 'react-redux'
import { depositMoney, getUserWallet, getWalletTransaction } from '@/State/Wallet/Action'
import { useLocation, useNavigate } from 'react-router-dom'


const Wallet = () => {
  const dispatch = useDispatch()
  const wallet = useSelector(state=>state.wallet)
  const location = useLocation()
  const navigate = useNavigate()
  const queryParams = new URLSearchParams(location.search)
  const orderId = queryParams.get("order_id")
  const paymentId = queryParams.get("paymentId")
  useEffect(()=>{
    handleFetchUserWallet()
    handleFetchWalletTransaction()
  },[])

  useEffect(()=>{
    if(orderId){
      dispatch(depositMoney({jwt:localStorage.getItem("jwt"),orderId,paymentId,navigate}))
    }
  },[orderId,paymentId])
  const handleFetchUserWallet = ()=>{
    dispatch(getUserWallet(localStorage.getItem("jwt")))
  }
  const handleFetchWalletTransaction = ()=>{
    dispatch(getWalletTransaction(localStorage.getItem("jwt")))
  }
  return (
    <div className='flex flex-col items-center'>
        <div className="pt-10 w-full lg:w-[60%]">
            <Card>
              <CardHeader className="pb-9">
                  <div className="flex justify-between items-center">
                      <div className="flex  items-center gap-5">
                          <WalletIcon size={30}/>
                          <div className="">
                              <CardTitle className="text-2xl">My Wallet</CardTitle>
                              <div className="flex items-center gap-2">
                                  <p className='text-gray-200 text-sm'>#{wallet.userWallet.id}</p>
                                  <CopyIcon size={15} className='cursor-pointer hover:text-slate-300'/>
                              </div>
                          </div>
                      </div>
                      <div className="">
                          <ReloadIcon onClick={handleFetchUserWallet} className='w-6 h-6 cursor-pointer hover:text-gray-400'/>
                      </div>
                  </div>
              </CardHeader>
              <CardContent>
                  <div className="flex items-center">
                      <DollarSign/>
                      <span className=' text-2xl font-semibold'>{wallet.userWallet.balance}</span>
                  </div>
                  <div className="flex gap-7 mt-5">
                      <Dialog>
                          <DialogTrigger  asChild>
                              <div  className="h-24 w-24 hover:text-gray-400 cursor-pointer flex flex-col items-center justify-center
                              rounded-md shadow-slate-800 shadow-md">
                                <UploadIcon/>
                                <span className='text-sm mt-2'>Add Money</span>
                              </div>
                          </DialogTrigger>
                          <DialogContent>
                              <DialogHeader>
                                <DialogTitle>Top Up Your Wallet</DialogTitle>
                              </DialogHeader>
                              <ToupForm/>
                          </DialogContent>
                      </Dialog>
                      <Dialog>
                          <DialogTrigger  asChild>
                              <div  className="h-24 w-24 hover:text-gray-400 cursor-pointer flex flex-col items-center justify-center
                              rounded-md shadow-slate-800 shadow-md">
                                <UploadIcon/>
                                <span className='text-sm mt-2'>Withdrawal</span>
                              </div>
                          </DialogTrigger>
                          <DialogContent>
                              <DialogHeader>
                                <DialogTitle>Request Withdrawal</DialogTitle>
                              </DialogHeader>
                              <WithdrawalForm />
                          </DialogContent>
                      </Dialog>
                      <Dialog>
                          <DialogTrigger  asChild>
                              <div  className="h-24 w-24 hover:text-gray-400 cursor-pointer flex flex-col items-center justify-center
                              rounded-md shadow-slate-800 shadow-md">
                                <ShuffleIcon/>
                                <span className='text-sm mt-2'>Transfer</span>
                              </div>
                          </DialogTrigger>
                          <DialogContent>
                              <DialogHeader>
                                <DialogTitle>Transfer to other wallet</DialogTitle>
                              </DialogHeader>
                              <TransferForm/>
                          </DialogContent>
                      </Dialog>
                  </div>
              </CardContent>
            </Card>

            <div className="py-5 mt-10">
                <div className="flex gap-2 items-center pb-5">
                    <h1 className='text-2xl font-semibold'>History</h1>
                    <UpdateIcon className='h-7 w-7 p-0 cursor-pointer hover:text-gray-400'/>
                </div>

                <div className="space-y-5">
                    {wallet.transactions.map(item=>(
                      <div className="">
                      <Card className=" px-5 flex justify-between items-center">
                        <div className='flex items-center gap-5'> 
                          <Avatar onClick={handleFetchWalletTransaction} className="flex flex-col justify-center items-center cursor-pointer hover:bg-slate-600">
                              <AvatarFallback>
                                  <ShuffleIcon className='h-7 w-7' />
                              </AvatarFallback>
                          </Avatar>

                          <div className="space-y-1">
                              <h3>{item.walletTransactionType==1?"WALLET_TRANSFER":item.walletTransactionType==2?"ADD_MONEY":"WITHDRAWAL"}</h3>
                              <p className='text-sm text-gray-500'>{item.datel}</p>
                          </div>
                          </div>
                          <div className="">
                            <p className={` text-green-500`}>{item.amount} USD</p>
                          </div>
                      </Card>
                  </div>
                    ))}
                </div>
            </div>
        </div>
    </div>
  )
}

export default Wallet
