using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Trading_coin.Models.Enum;
using Coin_Exchange.Models.Modal;

namespace Trading_coin.Models.Modal
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        // Liên kết tới User (một người dùng có nhiều Order)
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }

        public OrderType orderType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }

        public DateTime timestamps { get; set; } = DateTime.Now;

        public OrderStatus orderStatus { get; set; }

        
    }

}
