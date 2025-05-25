import axios from "axios"
import { FETCH_COIN_BY_ID_FAILURE, FETCH_COIN_BY_ID_REQUEST, FETCH_COIN_BY_ID_SUCCESS, FETCH_COIN_DETAILS_FAILURE, FETCH_COIN_DETAILS_REQUEST, FETCH_COIN_DETAILS_SUCCESS, FETCH_COIN_LIST_FAILURE, FETCH_COIN_LIST_REQUEST, FETCH_COIN_LIST_SUCCESS, FETCH_MARKET_CHART_FAILURE, FETCH_MARKET_CHART_REQUEST, FETCH_MARKET_CHART_SUCCESS, FETCH_TOP_50_COINS_FAILURE, FETCH_TOP_50_COINS_REQUEST, FETCH_TOP_50_COINS_SUCCESS, SEARCH_COIN_FAILURE, SEARCH_COIN_REQUEST, SEARCH_COIN_SUCCESS } from "./ActionType"

export const getCoinList = (page)=>async(dispatch)=>{
    dispatch({type:FETCH_COIN_LIST_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        const response = await axios.get(`${baseUrl}/coins?page=${page}`)
        const result = response.data
        console.log("list",result)
        dispatch({type:FETCH_COIN_LIST_SUCCESS,payload:result})
       
    }catch(error){

        dispatch({type:FETCH_COIN_LIST_FAILURE,payload:error.message})
        console.log(error)
    }
}


export const getTop50CoinList = ()=>async(dispatch)=>{
    dispatch({type:FETCH_TOP_50_COINS_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        const response = await axios.get(`${baseUrl}/coins/top50`)
        const result = response.data
        console.log("top50",result)
        dispatch({type:FETCH_TOP_50_COINS_SUCCESS,payload:result})
        

    }catch(error){

        dispatch({type:FETCH_TOP_50_COINS_FAILURE,payload:error.message})
        console.log(error)
    }
}

export const fetchMarketChart = ({coinId, days})=>async(dispatch)=>{
    dispatch({type:FETCH_MARKET_CHART_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        const response = await axios.get(`${baseUrl}/coins/${coinId}/chart?days=${days}`)
        const result = response.data
        console.log("market",result)
        dispatch({type:FETCH_MARKET_CHART_SUCCESS,payload:result})
        

    }catch(error){

        dispatch({type:FETCH_MARKET_CHART_FAILURE,payload:error.message})
        console.log(error)
    }
}

export const fetchCoinById = (coinId)=>async(dispatch)=>{
    dispatch({type:FETCH_COIN_BY_ID_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        const response = await axios.get(`${baseUrl}/coins/${coinId}`)
        const result = response.data
        console.log("coinById",result)
        dispatch({type:FETCH_COIN_BY_ID_SUCCESS,payload:result})
        

    }catch(error){

        dispatch({type:FETCH_COIN_BY_ID_FAILURE,payload:error.message})
        console.log(error)
    }
}

export const fetchCoinDetail = (coinId)=>async(dispatch)=>{
    dispatch({type:FETCH_COIN_DETAILS_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        const response = await axios.get(`${baseUrl}/coins/details/${coinId}`)
        const result = response.data
        console.log("coin detail",result)
        dispatch({type:FETCH_COIN_DETAILS_SUCCESS,payload:result})
        

    }catch(error){

        dispatch({type:FETCH_COIN_DETAILS_FAILURE,payload:error.message})
        console.log(error)
    }
}
export const searchCoin = (keyword)=>async(dispatch)=>{
    dispatch({type:SEARCH_COIN_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        const response = await axios.get(`${baseUrl}/coins/search?q=${keyword}`)
        const result = response.data
        console.log("search",result)
        dispatch({type:SEARCH_COIN_SUCCESS,payload:result})
        

    }catch(error){

        dispatch({type:SEARCH_COIN_FAILURE,payload:error.message})
        console.log(error)
    }
}