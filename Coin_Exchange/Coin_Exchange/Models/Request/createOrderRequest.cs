using Coin_Exchange.Models.Enum;

namespace Coin_Exchange.Models.Request
{
    public class createOrderRequest
    {
        public string coinId { set; get; }
        public string quantity { set; get; }
        public string orderType { set; get; }

        public double GetQuantityAsDouble()
        {
            double result;
            if (double.TryParse(quantity, out result))
            {
                return result;
            }
            throw new ArgumentException("Quantity is not a valid number.");
        }
    }
}
