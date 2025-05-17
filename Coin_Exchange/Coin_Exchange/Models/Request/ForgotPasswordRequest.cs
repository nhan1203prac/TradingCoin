using Microsoft.AspNetCore.Mvc;

namespace Coin_Exchange.Models.Request
{
    public class ForgotPasswordRequest
    {
        public string otp { get; set; }
    }
}
