using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace Trading_coin.Models.Modal
{
    public class Coin
    {
        [Key]
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("symbol")]
        public string symbol { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("image")]
        public string image { get; set; }

        [JsonProperty("current_price")]
        public double currentPrice { get; set; }

        [JsonProperty("market_cap")]
        public long marketCap { get; set; }

        [JsonProperty("market_cap_rank")]
        public int marketCapRank { get; set; }

        [JsonProperty("fully_diluted_valuation")]
        public double fullyDilutedValuation { get; set; }

        [JsonProperty("total_volume")]
        public long totalVolume { get; set; }

        [JsonProperty("high_24h")]
        public double high24h { get; set; }

        [JsonProperty("low_24h")]
        public double low24h { get; set; }

        [JsonProperty("price_change_24h")]
        public double priceChange24h { get; set; }

        [JsonProperty("price_change_percentage_24h")]
        public double priceChangePercentage24h { get; set; }

        [JsonProperty("market_cap_change_24h")]
        public long marketCapChange24h { get; set; }

        [JsonProperty("market_cap_change_percentage_24h")]
        public double marketCapChangePercentage24h { get; set; }

        public List<WatchlistCoin> WatchlistCoins { get; set; } = new List<WatchlistCoin>();
    }
}
