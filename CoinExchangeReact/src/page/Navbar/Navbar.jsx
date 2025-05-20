import React from 'react'
import {
    Sheet,
    SheetContent,
    SheetDescription,
    SheetHeader,
    SheetTitle,
    SheetTrigger,
  } from "@/components/ui/sheet"
import { Button } from '@/components/ui/button'
import { AvatarIcon, DragHandleHorizontalIcon, MagnifyingGlassIcon } from '@radix-ui/react-icons'
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import Sidebar from './Sidebar'
import { useSelector } from 'react-redux'

export default function Navbar() {
    const auth = useSelector((state) => state.auth)
  return (
    <div className='px-2 py-3 border-b  z-50 bg-background bg-opacity-0 sticky top-0 left-0 right-0 flex 
    justify-between items-center' style={{ width: '100vw' }}>
        <div className='flex items-center gap-3'>
            <Sheet key="left">
            <SheetTrigger asChild>
                <Button variant="outline" size="icon" className="rounded-full h-11 w-11 text-white ">
                    <DragHandleHorizontalIcon className='w-full'/>
                </Button>
            </SheetTrigger>
            <SheetContent side="left" className="w-72 border-r-0 flex flex-col ">
                <SheetHeader>
                <SheetTitle>
                <div className='text-3xl flex justify-center items-center gap-1 mt-8'>
                    <Avatar>
                        <AvatarImage src="https://cdn.gobankingrates.com/wp-content/uploads/2018/03/bitcoin-ethereum-cryptocurrency-taxes-blockchain-iStock-886921308.jpg?webp=1&w=1280&quality=75"/>
                        
                    </Avatar>
                    <div>
                        <span className='font-bold text-orange-700'>Crypto</span>
                        <span>Hub</span>
                    </div>
                </div>
                
                </SheetTitle>
                
                </SheetHeader>

                <Sidebar/>
            </SheetContent>
            </Sheet>

            <p className='text-sm lg:text-base cursor-pointer'>Crypto Trading</p>
            <div className="p-0 ml-9">
                <Button variant="outline" className="flex  items-center gap-3 text-white">
                    <MagnifyingGlassIcon/>
                    <span>Search</span>
                </Button>
            </div>
        </div>
        <div>
            <Avatar>
                <AvatarFallback>
                    {auth.user?.fullName[0].toUpperCase()}
                </AvatarFallback>
            </Avatar>
        </div>
    </div>
  )
}
