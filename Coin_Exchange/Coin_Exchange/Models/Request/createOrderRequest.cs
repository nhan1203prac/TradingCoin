using Coin_Exchange.Models.Enum;

namespace Coin_Exchange.Models.Request
{
    public class createOrderRequest
    {
        public string coinId { set; get; }
        public double quantity { set; get; }
        public OrderType orderType { set; get; }
    }
}
