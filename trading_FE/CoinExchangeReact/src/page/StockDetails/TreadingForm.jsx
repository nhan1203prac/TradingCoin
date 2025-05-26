import { Input } from '@/components/ui/input'
import React, { useEffect, useState } from 'react'
import { Avatar, AvatarImage } from '@/components/ui/avatar'
import { BookmarkFilledIcon, DotIcon } from '@radix-ui/react-icons'
import { Button } from '@/components/ui/button'
import { useDispatch, useSelector } from 'react-redux'
import { getAssetDetails } from '@/State/Asset/Action'
import { getUserWallet } from '@/State/Wallet/Action'
import { payOrder } from '@/State/Order/Action'
import { SheetClose } from '@/components/ui/sheet'
import { ToastContainer } from 'react-toastify'
const TreadingForm = ({data}) => {
  const [orderType,setOrderType] = useState("BUY")
  const [quantity,setQuantity] = useState(0)
  const [amount,setAmount] = useState(0)
  const {coin,wallet,asset} = useSelector(store=>store)
  const dispatch = useDispatch()

  useEffect(()=>{
    dispatch(getUserWallet(localStorage.getItem("jwt")))
    dispatch(getAssetDetails({coinId:data?.id || coin.coinDetails.id,jwt:localStorage.getItem("jwt")}))

  },[])
  const handleChange = (e)=>{
    const amount = e.target.value
    setAmount(amount)
    const volume = calculateByCost(amount,data.currentPrice || coin.coinDetails.currentPrice)
    setQuantity(volume)
  }

  const calculateByCost = (amount,price)=>{
    let volume = amount/price
    let decimalPlaces = Math.max(2,price.toString().split(".")[0].length)
    return volume.toFixed(decimalPlaces)
  }

  const handleBuyCrypto = ()=>{
    dispatch(payOrder({jwt:localStorage.getItem("jwt"),orderData:{
      coinId:data?.id || coin.coinDetails?.id,
      quantity,
      orderType
    },amount
  }))
  
  }
  console.log("trading coin", coin)
  return (
    <div className='space-y-10  pt-2' >
        <div className='w-full'>
         
            <div className='flex gap-2 items-center justify-between'>
                <Input className="py-7 focus:outline-none flex-1 " 
                placeholder="Enter Amount..."
                onChange={handleChange}
                type="number"
                name="amount"
                /> 

                <div className='w-36 h-14 border flex items-center justify-center rounded-md'>
                  <p className=' text-xl  '>{quantity}</p>

                </div>
            </div>

            {true && <h3 className='text-red-600 text-center pt-4'>Insufficient wallet balance to buy</h3>}
        </div>

        <div className="flex gap-5 items-center">
                <div className="">
                    <Avatar>
                        <AvatarImage src={data?.image || coin.coinDetails?.image || ''} />
                    </Avatar>
                </div>
                <div>
                    <div className="flex items-center gap-2">
                        <p>{data?.symbol?.toUpperCase() || coin.coinDetails?.symbol?.toUpperCase()}</p>
                        <DotIcon className='text-gray-400'/>
                        <p className='text-gray-400'>{data?.name || coin.coinDetails?.name}</p>
                    </div>
                    <div className="flex items-center gap-2">
                        <p className='text-xl font-bold'>${data?.currentPrice || coin.coinDetails.currentPrice}</p>
                        <p>
                          <span className='text-red-600'>
                              <span>{data?.marketCapChange24h || coin.coinDetails?.marketData?.marketCapChange24h}</span>
                              <span>({data?.marketCapChangePercentage24h || coin.coinDetails?.marketData?.marketCapChangePercentage24h}%)</span>
                          </span>
                        </p>
                    </div>
                </div>

                
            </div>

            <div className='flex items-center justify-between'>
                    <p>Order Type</p>
                    <p>Market Order</p>
                </div>
                <div className="flex items-center justify-between">
                  <p>{orderType=='BUY'?'Available Case': 'Available Quantity'}</p>
                  <p>{orderType=="BUY"?"$"+wallet.userWallet?.balance:asset.assetDetails?.quantity||0}</p>
                </div>
                <div>
                    <Button onClick={handleBuyCrypto} className={`w-full  py-6 ${orderType=="SELL"?"bg-red-600 text-white":""}`}>
                    {orderType}
                    </Button>
                    <Button variant="links" className="w-full mt-5 text-xl" onClick={()=>setOrderType(orderType=='BUY'?'SELL':'BUY')}>
                      {orderType=='SELL'?'Or BUY':'Or SELL'}
                    </Button>
                </div>
    </div>
  )
}

export default TreadingForm
