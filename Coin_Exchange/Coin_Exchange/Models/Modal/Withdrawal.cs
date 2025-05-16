using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Trading_coin.Models.Enum;
using Coin_Exchange.Models.Modal;

namespace Trading_coin.Models.Modal
{
    public class Withdrawal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public WithdrawalStatus status { get; set; }

        [ForeignKey("UserId")]
        public long userId { get; set; } 
        public virtual User user { get; set; } 

        public long amount { get; set; }

        public DateTime date { get; set; } = DateTime.Now;
    }
}
