using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coin_Exchange.Models.Enum;


namespace Coin_Exchange.Models.Modal
{
    public class WalletTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [ForeignKey("WalletId")]
        public virtual Wallet wallet { get; set; }

        public WalletTransactionType walletTransactionType { get; set; }
        public DateTime datel {  get; set; }
        public long transferId {  get; set; }
        public string purpose { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal amount { get; set; }
    }
}
