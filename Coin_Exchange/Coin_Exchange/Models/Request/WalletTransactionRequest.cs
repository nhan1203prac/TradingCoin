namespace Coin_Exchange.Models.Request
{
    public class WalletTransactionRequest
    {
        public string purpose { get; set; }

        public decimal amount { get; set; }
    }
}
