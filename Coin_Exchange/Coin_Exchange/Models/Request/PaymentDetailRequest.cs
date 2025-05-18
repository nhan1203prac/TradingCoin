namespace Coin_Exchange.Models.Request
{
    public class PaymentDetailRequest
    {
        public string accountNumber { get; set; }

        public string accountHolderName { get; set; }
        public string confirmAccountNumber { get; set; }
        public string ifsc { get; set; }

        public string bankName { get; set; }
    }
}
