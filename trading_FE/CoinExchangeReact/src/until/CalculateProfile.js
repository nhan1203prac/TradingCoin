export const CalculateProfile = (order)=>{
    if(order&&order?.buyPrice&&order?.sellPrice)
        return order.orderItem?.sellPrice-order.orderItem?.buyPrice
    return '-'
}