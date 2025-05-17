namespace Coin_Exchange.Models.Response
{
    public class AuthResponse
    {
        public string jwt { get; set; }
        public string message { get; set; }
        public Boolean isTwoFatorAuthEnabled { get; set; }
        public string session { get; set; }
    }
}
