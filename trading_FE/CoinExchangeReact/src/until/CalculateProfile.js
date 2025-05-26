export const CalculateProfile = (order)=>{
    if(order&&order?.buyPrice&&order?.sellPrice)
        return order?.sellPrice-order?.buyPrice
    return '-'
}