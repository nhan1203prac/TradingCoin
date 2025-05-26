import { ExitCoinInWatchlist } from "@/until/ExitsCoinInWatchlist"
import { GET_USER_WATCHLIST_REQUEST, ADD_COIN_TO_WATCHLIST_REQUEST, GET_USER_WATCHLIST_SUCCESS, ADD_COIN_TO_WATCHLIST_SUCCESS, ADD_COIN_TO_WATCHLIST_FAILURE, GET_USER_WATCHLIST_FAILURE }from "./ActionType"

const init = {
    watchlist:null,
    items:[],
    loading:false,
    error:null
}

const watchlistReducer = (state=init,action)=>{
    switch(action.type){
        case GET_USER_WATCHLIST_REQUEST:
        case ADD_COIN_TO_WATCHLIST_REQUEST:
            return{
                ...state,
                loading:true,
                error:null
            }

        case GET_USER_WATCHLIST_SUCCESS:
            return{
                ...state,
                watchlist:action.payload,
                items:action.payload.watchlistCoins,
                loading:false,
                error:null
            }
        
        case ADD_COIN_TO_WATCHLIST_SUCCESS:
            let updateItems = ExitCoinInWatchlist(state.items,action.payload)?state.items.filter(item=>item.id!==action.payload.id):[action.payload,...state.items]

            return{
                ...state,
                items:updateItems,
                loading:false,
                error:null
            }

        case ADD_COIN_TO_WATCHLIST_FAILURE:
        case GET_USER_WATCHLIST_FAILURE:
            return{
                ...state,
                loading:false,
                error:action.payload
            }
        default: return state
    }
}

export default watchlistReducer