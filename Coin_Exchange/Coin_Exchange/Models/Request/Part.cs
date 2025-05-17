namespace Coin_Exchange.Models.Request
{
    public class Part
    {
        public string Text { get; set; }
        public FunctionCall FunctionCall { get; set; }
        public FunctionResponseData FunctionResponse { get; set; }
    }
}
