import axios from "axios"
import { GET_ALL_ORDERS_FAILURE, GET_ALL_ORDERS_REQUEST, GET_ALL_ORDERS_SUCCESS, GET_ORDER_REQUEST, GET_ORDER_SUCCESS, PAY_ORDER_FAILURE, PAY_ORDER_REQUEST, PAY_ORDER_SUCCESS } from "./ActionType"


export const payOrder = ({jwt,orderData,amount})=>async(dispatch)=>{
    dispatch({type:PAY_ORDER_REQUEST})

    try {
        console.log("orderData", orderData)
        const res = await axios.post("http://localhost:8080/api/orders/pay",orderData,{
            headers:{
                Authorization: `Bearer ${jwt}`
            }
            
        })
        dispatch({type:PAY_ORDER_SUCCESS,payload:res.data})
        
        console.log("order success",res.data)
    } catch (error) {
        console.log("error", error)
        
        dispatch({type:PAY_ORDER_FAILURE,payload:error.message})
    }
}

export const getOrderById = ({jwt,orderId})=>async(dispatch)=>{
    dispatch({type:GET_ORDER_REQUEST})

    try {
        const res = await axios.get(`http://localhost:8080/api/orders/${orderId}`,{
            headers:{
                Authorization: `Bearer ${jwt}`
            }
        })
        dispatch({type:GET_ORDER_SUCCESS,payload:res.data,amount})
        console.log("Get order",res.data)
    } catch (error) {
        console.log("error", error)
        dispatch({type:PAY_ORDER_FAILURE,payload:res.data})
    }
}

export const getAllOrderForUser = ({jwt,orderType,assetSymbol})=>async(dispatch)=>{
    dispatch({type:GET_ALL_ORDERS_REQUEST})

    try {
        const params = {};
        if (orderType) params.orderType = orderType;
        if (assetSymbol) params.asset_symbol = assetSymbol;

        const res = await axios.get(`http://localhost:8080/api/orders`,{
            headers:{
                Authorization: `Bearer ${jwt}`
            },
            params
        })
        dispatch({type:GET_ALL_ORDERS_SUCCESS,payload:res.data})
        console.log("Get order",res.data)
    } catch (error) {
        console.log("error", error)
        dispatch({type:GET_ALL_ORDERS_FAILURE,payload:res.data})
    }
}