using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trading_coin.Models.Modal
{
    public class WatchlistCoin
    {
        public long Id { get; set; }

        [ForeignKey("Watchlist")]
        public long WatchlistId { get; set; }
        public Watchlist Watchlist { get; set; }

        [ForeignKey("Coin")]
        public string CoinId { get; set; }
        public Coin Coin { get; set; }
    }


}
