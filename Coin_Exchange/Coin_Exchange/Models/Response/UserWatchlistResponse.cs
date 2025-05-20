using Coin_Exchange.Models.Modal;

namespace Coin_Exchange.Models.Response
{
    public class UserWatchlistResponse
    {
        public long watchlistId { get; set; }


        public long userId { get; set; }
        public User user { get; set; }

        public List<WatchlistCoin> WatchlistCoins { get; set; } = new List<WatchlistCoin>();
    }
}
