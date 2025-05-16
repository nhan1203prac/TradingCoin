using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coin_Exchange.Models.Modal;
namespace Trading_coin.Models.Modal
{
    public class TwoFactorOtp
    {
        [Key]
        public string id { get; set; }
        public string otp { get; set; }

        [ForeignKey("User")]
        public User user { get; set; }
        public string jwt { get; set; }
       
    }
}
