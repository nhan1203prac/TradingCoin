import { PAY_ORDER_REQUEST, GET_ORDER_REQUEST, GET_ALL_ORDERS_REQUEST, PAY_ORDER_SUCCESS, GET_ORDER_SUCCESS, GET_ALL_ORDERS_SUCCESS, GET_ALL_ORDERS_FAILURE, GET_ORDER_FAILURE, PAY_ORDER_FAILURE } from "./ActionType"

const init = {
    order:null,
    orders:[],
    loading:false,
    error:null

}

const orderReducer = (state=init,action)=>{
    switch(action.type){
        case PAY_ORDER_REQUEST:
        case GET_ORDER_REQUEST:
        case GET_ALL_ORDERS_REQUEST:
            return {...state,loading:true,error:null}

        case PAY_ORDER_SUCCESS:
        case GET_ORDER_SUCCESS:
            return{
                ...state,
                order:action.payload,
                loading:false,
                error:null
            }

        case GET_ALL_ORDERS_SUCCESS:
            return{
                ...state,
                orders:action.payload,
                loading:false,
                error:null
           }

        case GET_ALL_ORDERS_FAILURE:
        case GET_ORDER_FAILURE:
        case PAY_ORDER_FAILURE:
            return{
                ...state,
                loading:false,
                error:action.payload
            }

        default:return state
    }
}

export default orderReducer