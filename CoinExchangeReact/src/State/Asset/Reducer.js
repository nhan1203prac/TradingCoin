
import { GET_ASSET_DETAILS_REQUEST, GET_USER_ASSETS_REQUEST, GET_ASSET_SUCCESS, GET_ASSET_DETAILS_SUCCESS, GET_USER_ASSETS_SUCCESS, GET_USER_ASSETS_FAILURE, GET_ASSET_DETAILS_FAILURE, GET_ASSET_FAILURE } from "./ActionType"

const init = {
    asset:null,
    userAssets:[],
    loading:false,
    error:null,
    assetDetails:null
}

const assetReducer = (state=init,action)=>{
    switch(action.type){
        case GET_ASSET_DETAILS_REQUEST:
        case GET_USER_ASSETS_REQUEST:
            return {...state,loading:true,error:null}
        case GET_ASSET_SUCCESS:
            return {...state,loading:false,asset:action.payload,error:null}
        case GET_ASSET_DETAILS_SUCCESS:
            return {...state,loading:false,assetDetails:action.payload,error:null}
        case GET_USER_ASSETS_SUCCESS:
            return {...state,loading:false,userAssets:action.payload,error:null}

        case GET_USER_ASSETS_FAILURE:
        case GET_ASSET_FAILURE:
            return {...state,loading:false,error:action.payload}

        default: return state
    }


}
export default assetReducer