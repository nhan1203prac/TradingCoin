using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Trading_coin.Models.Enum;
using Coin_Exchange.Models.Modal;

namespace Trading_coin.Models.Modal
{
    public class PaymentOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public long amount { get; set; }
        public PaymentOrderStatus status { get; set; }
        public PaymentMethod paymentMethod { get; set; }

        [ForeignKey("User")]
        public long userId { get; set; }
        public User user { get; set; }
    }
}
