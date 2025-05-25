import React, { useEffect, useState } from 'react'
import './Chatbot.css'
import { Button } from '@/components/ui/button'
import AssetTable from './AssetTable'
import StockChart from './StockChart'
import { Avatar, AvatarImage } from '@radix-ui/react-avatar'
import { Cross1Icon, DotIcon } from '@radix-ui/react-icons'
import { icons, MessageCircle } from 'lucide-react'
import { Input } from "@/components/ui/input"
import { useDispatch, useSelector } from 'react-redux'
import { getCoinList, getTop50CoinList } from '@/State/Coin/Action'
import {
    Pagination,
    PaginationContent,
    PaginationEllipsis,
    PaginationItem,
    PaginationLink,
    PaginationNext,
    PaginationPrevious,
  } from "@/components/ui/pagination"
import axios from 'axios'


const Home = () => {
    const [category,setCategory] = useState("all")
    const [inputValue,setInputValue] = useState("")
    const [isBotRelease, setIsBotRelease] = useState(false)
    const dispatch = useDispatch();
    const [message,setMessage] = useState([])
    const [isLoading, setIsLoading] = useState(false);
    const coin = useSelector(state=>state.coin)
    const handleCategory = (value)=>{
        setCategory(value)
    }
    const handleBotRelease = ()=>{
        setIsBotRelease(!isBotRelease)
    }
    const handleChange = (e)=>{
        setInputValue(e.target.value)
    }
    const handleKeyPress = async(event)=>{
        if(event.key=="Enter"){
            console.log(inputValue)
            setIsLoading(true)
            setMessage([...message,inputValue])
            
            setInputValue("")
            try {
                const response = await axios.post("http://localhost:8080/ai/chat",{Prompt:inputValue})
              
                console.error("ChatSuccess:", response?.data);
                setMessage(pre=>[...pre,response?.data?.message])
            } catch (error) {
                setMessage(pre=>[...pre,"Could not get response!"])
                console.error("send message error:", error.response?.data || error.message);
                
            } finally {
                setIsLoading(false);
            }
        
        }
        
    }
    useEffect(()=>{
        dispatch(getCoinList(1))
    },[])
    useEffect(()=>{
        if(category=="top50")
            dispatch(getTop50CoinList())
        
    },[category])
    // console.log("coin top 50", coin.top50)
  return (
    <div className='relative'>
        <div className="lg:flex">
            <div className="lg:w-[50%] lg:border-r">
                <div className="p-3 flex items-center gap-4">
                    <Button onClick = {()=>handleCategory("all")} variant = {category=="all"?"default":"outline"} className={`rounded-full ${category!="all"?"text-white":""}`}>All</Button>
                    <Button onClick = {()=>handleCategory("top50")} variant = {category=="top50"?"default":"outline"} className={`rounded-full ${category!="top50"?"text-white":""}`}>Top50</Button>
                    <Button onClick = {()=>handleCategory("topGainers")} variant = {category=="topGainers"?"default":"outline"} className={`rounded-full ${category!="topGainers"?"text-white":""}`}>TopGainers</Button>
                    <Button onClick = {()=>handleCategory("topLosers")} variant = {category=="topLosers"?"default":"outline"} className={`rounded-full ${category!="topLosers"?"text-white":""}`}>TopLosers</Button>
                </div> 
                <AssetTable coin={category=="all"?coin.coinList:coin.top50} category={category}/>
                <div>
                <Pagination>
                <PaginationContent>
                    <PaginationItem>
                    <PaginationPrevious href="#" />
                    </PaginationItem>
                    <PaginationItem>
                    <PaginationLink href="#">1</PaginationLink>
                    </PaginationItem>
                    <PaginationItem>
                    <PaginationEllipsis />
                    </PaginationItem>
                    <PaginationItem>
                    <PaginationNext href="#" />
                    </PaginationItem>
                </PaginationContent>
                </Pagination>

                </div>
            </div>  

            <div className="hidden lg:block lg:w-[50%] p-5">
                <StockChart coinId={"bitcoin"}/>
                {coin.coinList && coin.coinList.length > 0 && (
                    <div className="flex gap-5 items-center mt-5">
                    <div>
                        <Avatar>
                            <AvatarImage src="https://www.google.com/url?sa=i&url=https%3A%2F%2Fcoinmarketcap.com%2Fvi%2Fcurrencies%2Fethereum%2F&psig=AOvVaw1XNdzQOr8xWA0Qe6oRare8&ust=1730181811516000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCNDmzeazsIkDFQAAAAAdAAAAABAE"/>    
                        </Avatar>    
                    </div>
                <div>
                    <div className='flex items-center gap-2'>
                        <p>{coin.coinList[0].symbol?.toUpperCase()}</p>
                        <DotIcon className='text-gray-400'/>
                        <p className='text-gray-400'>{coin.coinList[0].name}</p>    
                    </div>  
                    <div className='flex items-end gap-2'>
                        <p className='text-xl font-bold'>{coin.coinList[0].current_price}</p>
                        <p className='text-red-600'>
                            <span>{coin.coinList[0].market_cap_change_24h}</span>
                            <span>({coin.coinList[0].market_cap_change_percentage_24h}%)</span>
                        </p>
                    </div>  
               </div>
                </div>  
                )}  
            </div>  
        </div>
        <section className='absolute bottom-0 right-5 z-40 flex flex-col justify-end items-end gap-2'>
            {isBotRelease&&<div className="rounded-md w-[20rem] md:w-[25rem] lg:w-[25rem] h-[70vh] bg-slate-900">
                <div className="flex justify-between items-center border-b px-6 h-[12%]">
                    <p>Chat Bot</p>
                    <Button onClick = {handleBotRelease} variant="ghost" size="icon">
                        <Cross1Icon/>
                    </Button>
                </div>
                <div className="h-[76%] flex flex-col overflow-y-auto gap-5 px-5 py-2 scroll-container">
                   <div className="self-start pb-5 w-auto">
                        <div className="justify-end self-end px-5 py-2 rounded-md bg-slate-800 w-auto">
                                <p>hi, Vo Thanh Nhan</p>
                                <p>you can ask crypto related any question</p>
                                <p>like, price, market cap extra...</p>
                        </div>
                   </div>


                   {message.map((item,i)=>(
                        <div className={`${i%2!=0?"self-start":"self-end"} pb-5 w-auto`} key={i}>
                            <div className="justify-end self-end px-5 py-2 rounded-md bg-slate-800 w-auto">
                                    <p>{item}</p>
                                    </div>
                                    

                            
                        </div>
                   ))}
                   {isLoading && (
                <div className="self-start pb-5 w-auto">
                  <div className="px-5 py-2 rounded-md bg-slate-800 w-auto">
                    <div className="loading-dots">
                      <span></span>
                      <span></span>
                      <span></span>
                    </div>
                  </div>
                </div>
              )}
                </div>
                <div className="h-[12%] border-t">
                    <Input className="w-full h-full border-none outline-none" placeholder="write prompt"
                    onChange={handleChange} value = {inputValue} onKeyPress={handleKeyPress}
                    />
                </div>
            </div>
            }
            <div className='relative w-[10rem] cursor-pointer group'>
                <Button className="w-full h-[3rem] gap-2 items-center" onClick = {handleBotRelease}>
                    <MessageCircle size={30} className='fill-[#1e293b] -rotate-90  stroke-none group-hover:fill-[#1a1a1a]'/>
                    <span className='text-xl'>Chat Bot</span>
                </Button>
            </div>
        </section> 
    </div>
  )
}

export default Home
