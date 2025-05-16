using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using Coin_Exchange.Models.Modal;
namespace Trading_coin.Models.Modal
{
    public class Wallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [ForeignKey("UserId")]
        public virtual User user { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal balance { get; set; } = 0m;
    }
}
