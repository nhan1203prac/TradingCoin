using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coin_Exchange.Models.Response
{
    public class OrderItemDTO
    {
        public long Id { get; set; }
        public double Quantity { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public Coin Coin { get; set; }
        public string orderType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }
        public DateTime Timestamps { get; set; }
    }
}
