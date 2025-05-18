using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models.Response;
using Coin_Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Coin_Exchange.Controllers
{
    [Route("coins/")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly HttpClient _httpClient = new HttpClient();

        public CoinController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Coin>>> getListCoin([FromQuery] int page)
        {

            List<Coin> coinList = await _context.Coins
             .OrderBy(c => c.marketCapRank)
             .Take(10)
             .ToListAsync();


            return Ok(coinList);
        }


        [HttpGet("details/{coinId}")]
        public async Task<ActionResult<Coin>> getCoinDetails(string coinId)
        {

            Coin coin = await _context.Coins.FirstOrDefaultAsync(c => c.id == coinId);

            return Ok(coin);
        }


        [HttpGet("top50")]
        public async Task<ActionResult<List<Coin>>> getTop50CoinsByMarketCapRank()
        {

            List<Coin> coin = await _context.Coins.OrderBy(c => c.marketCapRank).ToListAsync();

            return Ok(coin);
        }
        [HttpGet("/{coinId}")]
        public async Task<ActionResult<AuthResponse>> getCoinById(string coinId)
        {

            Coin coin = await _context.Coins.FirstOrDefaultAsync(c => c.id == coinId);

            return Ok(coin);
        }

        [HttpGet("{coinId}/chart")]
        public async Task<string> GetMarketChartAsync(string coinId, [FromQuery] int days)
        {
            string url = $"https://api.coingecko.com/api/v3/coins/{coinId}/market_chart?vs_currency=usd&days={days}";
            int maxRetries = 3;
            int retryCount = 0;
            int waitTime = 10000;

            while (retryCount < maxRetries)
            {
                try
                {

                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();


                    string responseBody = await response.Content.ReadAsStringAsync();


                    JObject root = JObject.Parse(responseBody);
                    JArray prices = (JArray)root["prices"];


                    JArray filteredPrices = new JArray();
                    for (int i = 0; i < prices.Count; i += 5)
                    {
                        filteredPrices.Add(prices[i]);
                    }


                    return JsonConvert.SerializeObject(filteredPrices);
                }
                catch (HttpRequestException e) when ((int)(e.StatusCode ?? 0) == 429)
                {
                    Console.WriteLine("Rate limit exceeded. Waiting before retrying...");
                    await Task.Delay(waitTime);
                    retryCount++;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred: {ex.Message}");
                }
            }

            throw new Exception("Exceeded maximum retries for rate limit.");
        }

    }
}
