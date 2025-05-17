using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Coin_Exchange.Models.Modal
{
    public class PaymentDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public string accountNumber { get; set; }

        public string accountHolderName { get; set; }

        public string ifsc { get; set; }

        public string bankName { get; set; }

        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}
