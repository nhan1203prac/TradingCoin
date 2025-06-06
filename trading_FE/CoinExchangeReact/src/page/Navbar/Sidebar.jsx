import { Button } from '@/components/ui/button'
import { SheetClose } from '@/components/ui/sheet'
import { ActivityLogIcon, BookmarkIcon, DashboardIcon, ExitIcon, HomeIcon, PersonIcon } from '@radix-ui/react-icons'
import { CreditCardIcon, LandmarkIcon, SettingsIcon, WalletIcon } from 'lucide-react'
import './Sidebar.css'
import React from 'react'
import { useNavigate } from 'react-router-dom'
import { useDispatch } from 'react-redux'
import { logout } from '@/State/Auth/Action'

const menu=[
  {name:"Admin proceed",path:"/admin/withdrawal",icon:<SettingsIcon className='h-6 w-6'/>},
  {name:"Home",path:"/",icon:<HomeIcon className='h-6 w-6'/>},
  {name:"Portfolio",path:"/portfolio",icon:<DashboardIcon className='h-6 w-6'/>},
  {name:"Watchlist",path:"/watchlist",icon:<BookmarkIcon className='h-6 w-6'/>},
  {name:"Activity",path:"/activity",icon:<ActivityLogIcon className='h-6 w-6'/>},
  {name:"Wallet",path:"/wallet",icon:<WalletIcon className='h-6 w-6'/>},
  {name:"Payment Details",path:"/payment-details",icon:<LandmarkIcon className='h-6 w-6'/>},
  {name:"Withdrawal",path:"/withdrawal",icon:<CreditCardIcon className='h-6 w-6'/>},
  {name:"Profile",path:"/profile",icon:<PersonIcon className='h-6 w-6'/>},
  {name:"Logout",path:"/",icon:<ExitIcon className='h-6 w-6'/>}
]
function Sidebar() {
  const navigate = useNavigate()
  const dispatch = useDispatch()

  const handleLogout = ()=>{
    dispatch(logout())
  }
  return (
    <div className='mt-3 custom-scrollbar max-h-screen overflow-y-auto  '>

      {menu.map(item=>(
         <div key={item.name}>
         <SheetClose className='w-full bg-transparent  hover:border-none focus-visible:border-none focus:border-none' >

           <Button variant="outline" className="flex items-center gap-5 py-6 w-full text-white" 
           onClick={()=>{navigate(item.path) 
            if(item.name=="Logout"){handleLogout()}}}>
               <span className='w-8'>
                   {item.icon}
               </span>
               <p>{item.name}</p>
           </Button>
           
         </SheetClose>
       </div>
      ))}
       
    </div>
  )
}

export default Sidebar
