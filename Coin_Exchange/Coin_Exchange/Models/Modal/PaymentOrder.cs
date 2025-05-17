using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models.Enum;

namespace Coin_Exchange.Models.Modal
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
