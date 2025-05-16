using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coin_Exchange.Models.Modal;

namespace Trading_coin.Models.Modal
{
    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long id { get; set; }

        public double quantity { get; set; }
        public double buyPrice { get; set; }

   
        [ForeignKey("CoinId")]
        public virtual Coin coin { get; set; }

        [ForeignKey("UserId")]
        public virtual User user { get; set; }
    }
}
