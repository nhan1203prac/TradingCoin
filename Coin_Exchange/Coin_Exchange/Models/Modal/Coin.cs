using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Coin_Exchange.Models.Modal
{
    public class Coin
    {
        [Key]
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("symbol")]
        public string symbol { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("image")]
        public string image { get; set; }

        [JsonPropertyName("current_price")]
        public double currentPrice { get; set; }

        [JsonPropertyName("market_cap")]
        public long marketCap { get; set; }

        [JsonPropertyName("market_cap_rank")]
        public int marketCapRank { get; set; }

        [JsonPropertyName("fully_diluted_valuation")]
        public double fullyDilutedValuation { get; set; }

        [JsonPropertyName("total_volume")]
        public long totalVolume { get; set; }

        [JsonPropertyName("high_24h")]
        public double high24h { get; set; }

        [JsonPropertyName("low_24h")]
        public double low24h { get; set; }

        [JsonPropertyName("price_change_24h")]
        public double priceChange24h { get; set; }

        [JsonPropertyName("price_change_percentage_24h")]
        public double priceChangePercentage24h { get; set; }

        [JsonPropertyName("market_cap_change_24h")]
        public long marketCapChange24h { get; set; }

        [JsonPropertyName("market_cap_change_percentage_24h")]
        public double marketCapChangePercentage24h { get; set; }

        public List<WatchlistCoin> WatchlistCoins { get; set; } = new List<WatchlistCoin>();
    }
}
