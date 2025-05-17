using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Coin_Exchange.Models.Modal;

namespace Coin_Exchange.Models.Modal
{
    public class Watchlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [ForeignKey("User")]
        public long userId { get; set; } 
        public User user { get; set; }
        [JsonIgnore]
        public List<WatchlistCoin> WatchlistCoins { get; set; } = new List<WatchlistCoin>();
    }
}
