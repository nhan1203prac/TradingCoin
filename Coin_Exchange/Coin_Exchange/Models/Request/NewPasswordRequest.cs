using Microsoft.AspNetCore.Mvc;

namespace Coin_Exchange.Models.Request
{
    public class NewPasswordRequest
    {
        public string password { get; set; }
        public string email { get; set; }
    }
}
