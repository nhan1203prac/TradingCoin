import { Button } from './components/ui/button'
import Home from './page/Home/Home'
import Navbar from './page/Navbar/Navbar'
import { Route, Routes } from 'react-router-dom' // Đảm bảo import cả Routes

import Notfound from './page/Notfound/Notfound'

import Auth from './page/auth/Auth'
import { useDispatch, useSelector } from 'react-redux'
import { useEffect } from 'react'
import { getUser } from './State/Auth/Action'
import Portfolio from './page/Portfolio/Portfolio'
import WatchList from './page/WatchList/WatchList'
import Activity from './page/Activity/Activity'
import Wallet from './page/Wallet/Wallet'
import PaymentDetails from './page/PaymentDetails/PaymentDetails'
import Withdrawal from './page/Withdrawal/Withdrawal'
import StockDetails from './page/StockDetails/StockDetails'
import Profile from './page/Profile/Profile'
import WithdrawalAdmin from './page/Admin/WithdrawalAdmin'

function App() {
    const { auth } = useSelector(state => state); // Lấy cả state
    const dispatch = useDispatch();

    useEffect(() => {
        const storedJwt = localStorage.getItem("jwt");
        if (storedJwt) {
            dispatch(getUser(storedJwt));
        }
    }, [dispatch, auth.jwt]); 

    return (
        <>
           
            {auth.user?.role === 0 ? ( 
                <div>
                   <Navbar />
                   <Routes>
                        
                        <Route path='/' element={<Home />} />
                        <Route path='/admin/withdrawal' element={<WithdrawalAdmin />} />
                        <Route path='/portfolio' element={<Portfolio />} />
                        <Route path='/activity' element={<Activity />} />
                        <Route path='/wallet' element={<Wallet />} />
                        <Route path='/withdrawal' element={<Withdrawal />} />
                        <Route path='/payment-details' element={<PaymentDetails />} />
                        <Route path='/market/:id' element={<StockDetails />} />
                        <Route path='/watchlist' element={<WatchList />} />
                        <Route path='/profile' element={<Profile />} />
                       
                        <Route path='*' element={<Notfound />} /> 
                </Routes>
                </div>
                
            ) : auth.user ? (
              
                <div>
                   <Navbar />
                    
                    <Routes> 
                        <Route path='/' element={<Home />} />
                        <Route path='/portfolio' element={<Portfolio />} />
                        <Route path='/activity' element={<Activity />} />
                        <Route path='/wallet' element={<Wallet />} />
                        <Route path='/withdrawal' element={<Withdrawal />} />
                        <Route path='/payment-details' element={<PaymentDetails />} />
                        <Route path='/market/:id' element={<StockDetails />} />
                        <Route path='/watchlist' element={<WatchList />} />
                        <Route path='/profile' element={<Profile />} />
                       
                        <Route path='*' element={<Notfound />} /> 
                    </Routes>
                </div>
            ) : (
                
                <Auth />
            )}
        </>
    );
}

export default App;