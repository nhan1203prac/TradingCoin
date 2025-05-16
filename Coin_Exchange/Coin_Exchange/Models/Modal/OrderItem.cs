using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Trading_coin.Models.Modal
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public double quantity { get; set; }

        [ForeignKey("CoinId")]
        public Coin coin { get; set; }  

        public double buyPrice { get; set; }

        public double sellPrice { get; set; }

       
        [ForeignKey("OrderId")]
        public virtual Order order { get; set; }

    }
}
