using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coin_Exchange.Models.Modal;

namespace Trading_coin.Models.Modal
{
    public class ForgotPasswordToken
    {
        [Key]
        public string id { get; set; }  
       

        [ForeignKey("UserId")]
        public virtual User user { get; set; }

        public string otp { get; set; }
        public string sendTo { get; set; }
    }
}
