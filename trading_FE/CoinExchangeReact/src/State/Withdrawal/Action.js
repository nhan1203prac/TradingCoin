import axios from "axios";
import { ADD_PAYMENT_DETAILS_FAILURE, ADD_PAYMENT_DETAILS_REQUEST, ADD_PAYMENT_DETAILS_SUCCESS, GET_PAYMENT_DETAILS_FAILURE, GET_PAYMENT_DETAILS_REQUEST, GET_PAYMENT_DETAILS_SUCCESS, GET_WITHDRAWAL_HISTORY_FAILURE, GET_WITHDRAWAL_HISTORY_REQUEST, GET_WITHDRAWAL_HISTORY_SUCCESS, GET_WITHDRAWAL_REQUEST_FAILURE, GET_WITHDRAWAL_REQUEST_REQUEST, GET_WITHDRAWAL_REQUEST_SUCCESS, WITHDRAWAL_FAILURE, WITHDRAWAL_PROCEED_FAILURE, WITHDRAWAL_PROCEED_REQUEST, WITHDRAWAL_PROCEED_SUCCESS, WITHDRAWAL_REQUEST, WITHDRAWAL_SUCCESS } from "./ActionType";

export const withdrawalRequest = ({amount,jwt})=>async(dispatch)=>{
    dispatch({type:WITHDRAWAL_REQUEST});

    try {
        const response = await axios.post(`http://localhost:8080/api/withdrawal/${amount}`,null,{
            headers:{
                Authorization:`Bearer ${jwt}`
            }
        })
        console.log("withdrawal", response.data)
        dispatch({type:WITHDRAWAL_SUCCESS,payload:response.data})
    } catch (error) {
        dispatch({type:WITHDRAWAL_FAILURE,payload:error.message})
    }
}


export const proceedWithdrawal = ({id,jwt,accept})=>async(dispatch)=>{
    dispatch({type:WITHDRAWAL_PROCEED_REQUEST});

    try {
        const response = await axios.patch(`http://localhost:8080/api/admin/withdrawal/${id}/proceed/${accept}`,null,{
            headers:{
                Authorization:`Bearer ${jwt}`
            }
        })
        console.log("proceed withdrawal", response.data)
        dispatch({type:WITHDRAWAL_PROCEED_SUCCESS,payload:response.data})
    } catch (error) {
        dispatch({type:WITHDRAWAL_PROCEED_FAILURE,payload:error.message})
    }
}


export const getWithdrawalHistory = ({jwt})=>async(dispatch)=>{
    dispatch({type:GET_WITHDRAWAL_HISTORY_REQUEST});

    try {
        const response = await axios.get(`http://localhost:8080/api/withdrawal`,{
            headers:{
                Authorization:`Bearer ${jwt}`
            }
        })
        console.log("withdrawal history", response.data)
        dispatch({type:GET_WITHDRAWAL_HISTORY_SUCCESS,payload:response.data})
    } catch (error) {
        dispatch({type:GET_WITHDRAWAL_HISTORY_FAILURE,payload:error.message})
    }
}


export const getAllWithdrawalRequest = ({jwt})=>async(dispatch)=>{
    dispatch({type:GET_WITHDRAWAL_REQUEST_REQUEST});

    try {
        const response = await axios.get(`http://localhost:8080/api/admin/withdrawal`,{
            headers:{
                Authorization:`Bearer ${jwt}`
            }
        }) 
        console.log("get withdrawal request", response.data)
        dispatch({type:GET_WITHDRAWAL_REQUEST_SUCCESS,payload:response.data})
    } catch (error) {
        dispatch({type:GET_WITHDRAWAL_REQUEST_FAILURE,payload:error.message})
    }
}


export const addPaymentDetails = ({paymentDetail,jwt})=>async(dispatch)=>{
    dispatch({type:ADD_PAYMENT_DETAILS_REQUEST});

    try {
        const response = await axios.post(`http://localhost:8080/api/payment-details`,paymentDetail,{
            headers:{
                Authorization:`Bearer ${jwt}`
            }
        }) 
        console.log("add payment detail", response.data)
        dispatch({type:ADD_PAYMENT_DETAILS_SUCCESS,payload:response.data})
    } catch (error) {
        dispatch({type:ADD_PAYMENT_DETAILS_FAILURE,payload:error.message})
    }
}



export const getPaymentDetail = ({jwt})=>async(dispatch)=>{
    dispatch({type:GET_PAYMENT_DETAILS_REQUEST});

    try {
        const response = await axios.get(`http://localhost:8080/api/payment-details`,{
            headers:{
                Authorization:`Bearer ${jwt}`
            }
        }) 
        console.log("get payment detail", response.data)
        dispatch({type:GET_PAYMENT_DETAILS_SUCCESS,payload:response.data})
    } catch (error) {
        dispatch({type:GET_PAYMENT_DETAILS_FAILURE,payload:error.message})
    }
}