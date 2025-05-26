import axios from "axios"
import { GET_ASSET_DETAILS_FAILURE, GET_ASSET_DETAILS_REQUEST, GET_ASSET_DETAILS_SUCCESS, GET_ASSET_FAILURE, GET_ASSET_REQUEST, GET_ASSET_SUCCESS, GET_USER_ASSETS_FAILURE, GET_USER_ASSETS_REQUEST, GET_USER_ASSETS_SUCCESS } from "./ActionType"

export const getAssetById = ({assetId,jwt})=>async(dispatch)=>{
    dispatch({type:GET_ASSET_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        const response = await axios.get(`${baseUrl}/api/assets/${assetId}`,{
            headers:{
                Authorization: `Bearer ${jwt}`
            }
        })
       
        dispatch({type:GET_ASSET_SUCCESS,payload:response.data})
       console.log("get asset By id", response.data)
    }catch(error){

        dispatch({type:GET_ASSET_FAILURE,payload:error.message})
        console.log(error)
    }
}


export const getAssetDetails = ({coinId,jwt})=>async(dispatch)=>{
    dispatch({type:GET_ASSET_DETAILS_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        console.log("coinId", coinId)
        console.log("jwt", jwt)
        const response = await axios.get(`${baseUrl}/api/assets/coin/${coinId}/user`,{
            headers:{
                Authorization: `Bearer ${jwt}`
            }
        })
       
        dispatch({type:GET_ASSET_DETAILS_SUCCESS,payload:response.data})
       console.log("get asset detail", response.data)
    }catch(error){

        dispatch({type:GET_ASSET_DETAILS_FAILURE,payload:error.message})
        console.log(error)
    }
}


export const getUserAssets = ({jwt})=>async(dispatch)=>{
    dispatch({type:GET_USER_ASSETS_REQUEST})
    const baseUrl = "http://localhost:8080"

    try{
        const response = await axios.get(`${baseUrl}/api/assets`,{
            headers:{
                Authorization: `Bearer ${jwt}`
            }
        })
       
        dispatch({type:GET_USER_ASSETS_SUCCESS,payload:response.data})
       console.log("get asset detail", response.data)
    }catch(error){

        dispatch({type:GET_USER_ASSETS_FAILURE,payload:error.message})
        console.log(error)
    }
}
