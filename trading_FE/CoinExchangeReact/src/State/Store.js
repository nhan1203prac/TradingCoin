import { thunk } from "redux-thunk";
import authReducer from "./Auth/Reducer";

import { combineReducers, legacy_createStore, applyMiddleware } from 'redux';
import coinReducer from "./Coin/Reducer";
import assetReducer from "./Asset/Reducer";
import watchlistReducer from "./Watchlist/Reducer";
import orderReducer from "./Order/Reducer";
import walletReducer from "./Wallet/Reducer";
import withdrawalReducer from "./Withdrawal/Reducer";



const rootReducer = combineReducers({
    auth:authReducer,
    coin:coinReducer,
    asset:assetReducer,
    watchlist:watchlistReducer,
    order:orderReducer,
    wallet:walletReducer,
    withdrawal:withdrawalReducer
})

export const store = legacy_createStore(rootReducer,applyMiddleware(thunk))