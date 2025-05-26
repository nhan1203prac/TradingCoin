


import { Button } from './components/ui/button'
import Home from './page/Home/Home'
import Navbar from './page/Navbar/Navbar'
import { Route,Routes } from 'react-router-dom'
import Portfolio from './page/Portfolio/Portfolio'
import Withdrawal from './page/Withdrawal/Withdrawal'
import Wallet from './page/Wallet/Wallet'
import PaymentDetails from './page/PaymentDetails/PaymentDetails'
import StockDetails from './page/StockDetails/StockDetails'
import WatchList from './page/WatchList/WatchList'
import SearchCoin from './page/Search/SearchCoin'
import Notfound from './page/Notfound/Notfound'
import Profile from './page/Profile/Profile'
import Activity from './page/Activity/Activity'
import Auth from './page/auth/Auth'
import { useDispatch, useSelector } from 'react-redux'
import { useEffect } from 'react'
import { getUser } from './State/Auth/Action'

function App() {
  const auth = useSelector(state => state.auth);

  console.log("Auth----" ,auth)

  const dispatch = useDispatch()

  useEffect(() => {
    const storedJwt = localStorage.getItem("jwt");  
    if (storedJwt) {
      
      dispatch(getUser(storedJwt));
    }
  }, [auth.jwt]);
console.log("user---",auth.jwt)
  return (
    <>
    
   
      {auth.user ? <div>
        <Navbar/>
      <Routes>
        <Route path='/' element={<Home/>}/>
        <Route path='/portfolio' element={<Portfolio/>} />
        <Route path='/activity' element={<Activity/>} />
        <Route path='/wallet' element={<Wallet/>} />
        <Route path='/withdrawal' element={<Withdrawal/>} />
        <Route path='/payment-details' element={<PaymentDetails/>} />
        <Route path='/market/:id' element={<StockDetails/>} />
        <Route path='/watchlist' element={<WatchList/>} />
        <Route path='/profile' element={<Profile/>} />
        <Route path='/search' element={<SearchCoin/>} />
        <Route path='*' element={<Notfound/>} />


      </Routes>
        </div>:<Auth/>}
     
      
    </>
  )
}

export default App
