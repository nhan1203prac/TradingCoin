namespace Coin_Exchange.Models.Response
{
    public class CoinDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Image { get; set; }
        public double CurrentPrice { get; set; }
        public double MarketCap { get; set; }
        public double MarketCapRank { get; set; }
        public double TotalVolume { get; set; }
        public double High24h { get; set; }
        public double Low24h { get; set; }
        public double PriceChange24h { get; set; }
        public double PriceChangePercentage24h { get; set; }
        public double MarketCapChange24h { get; set; }
        public double MarketCapChangePercentage24h { get; set; }
        public double CirculatingSupply { get; set; }
        public double TotalSupply { get; set; }
        public long Ath { get; set; }
        public long AthChangePercentage { get; set; }
        public DateTime? AthDate { get; set; }
        public long Atl { get; set; }
        public long AtlChangePercentage { get; set; }
        public DateTime? AtlDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
