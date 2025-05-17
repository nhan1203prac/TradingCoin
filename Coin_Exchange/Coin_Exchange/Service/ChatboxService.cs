using Coin_Exchange.Models.Request;
using Coin_Exchange.Models.Response;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coin_Exchange.Service
{
    public class ChatboxService : IChatboxService
    {
        private readonly string GEMINI_API_KEY;
        private readonly HttpClient _httpClient;

        public ChatboxService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            GEMINI_API_KEY = configuration["GeminiApiKey"];
        }
        public Double ConvertDouble(Object value)
        {
            if (value is int intValue) return (double)intValue;
            else if (value is long longValue) return (double)longValue;
            else if (value is double doubleValue) return doubleValue;
            else
            {
                throw new InvalidCastException($"Unsupported type: {value.GetType().Name}");
            }
        }

        public async Task<CoinDto> MakeApiRequest(string currencyName)
        {
            Console.WriteLine($"currencyNam: {currencyName}");
            string url = $"https://api.coingecko.com/api/v3/coins/{currencyName}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string,object>>();

            if (responseBody != null)
            {
                var image = (JsonElement)responseBody["image"];
                var marketData = (JsonElement)responseBody["market_data"];

                var coinDto = new CoinDto
                {
                    Id = responseBody["id"].ToString(),
                    Name = responseBody["name"].ToString(),
                    Symbol = responseBody["symbol"].ToString(),
                    Image = image.GetProperty("large").GetString(),
                    CurrentPrice = ConvertDouble(marketData.GetProperty("current_price").GetProperty("usd").GetDouble()),
                    MarketCap = ConvertDouble(marketData.GetProperty("market_cap").GetProperty("usd").GetDouble()),
                    MarketCapRank = ConvertDouble(marketData.GetProperty("market_cap_rank").GetDouble()),
                    TotalVolume = ConvertDouble(marketData.GetProperty("total_volume").GetProperty("usd").GetDouble()),
                    High24h = ConvertDouble(marketData.GetProperty("high_24h").GetProperty("usd").GetDouble()),
                    Low24h = ConvertDouble(marketData.GetProperty("low_24h").GetProperty("usd").GetDouble()),
                    PriceChange24h = ConvertDouble(marketData.GetProperty("price_change_24h").GetDouble()),
                    PriceChangePercentage24h = ConvertDouble(marketData.GetProperty("price_change_percentage_24h").GetDouble()),
                    MarketCapChange24h = ConvertDouble(marketData.GetProperty("market_cap_change_24h").GetDouble()),
                    MarketCapChangePercentage24h = ConvertDouble(marketData.GetProperty("market_cap_change_percentage_24h").GetDouble()),
                    CirculatingSupply = ConvertDouble(marketData.GetProperty("circulating_supply").GetDouble()),
                    TotalSupply = ConvertDouble(marketData.GetProperty("total_supply").GetDouble())
                };

                return coinDto;
            }

            throw new Exception("Coin not found");
        }

        public async Task<FunctionResponse> GetFunctionResponse(string prompt)
        {
            string geminiApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={GEMINI_API_KEY}";
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user", // Thay "content" bằng "role" và "parts"
                        parts = new[]
                        {
                            new { text = $"Extract the coin ID (e.g., 'bitcoin', 'ethereum') from this query: {prompt}" }
                        }
                    }
                },
                tools = new[]
                {
                    new
                    {
                        functionDeclarations = new[]
                        {
                            new
                            {
                                name = "getCoinDetails",
                                description = "Get the coin details based on the coin ID",
                                parameters = new
                                {
                                    type = "object",
                                    properties = new
                                    {
                                        currencyName = new
                                        {
                                            type = "string",
                                            description = "The coin ID (e.g., 'bitcoin', 'ethereum')"
                                        }
                                    },
                                    required = new[] { "currencyName" }
                                }
                            }
                        }
                    }
                },
                toolConfig = new
                {
                    functionCallingConfig = new
                    {
                        mode = "ANY",
                        allowedFunctionNames = new[] { "getCoinDetails" }
                    }
                }
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(geminiApiUrl, content);
            //response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Gemini API Error Response: {errorBody}");
                throw new HttpRequestException($"Gemini API request failed: {response.StatusCode}, {errorBody}");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Gemini API response: {responseBody}");

            var jsonDoc = JsonDocument.Parse(responseBody);
            var functionCall = jsonDoc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("functionCall");

            string functionName = functionCall.GetProperty("name").GetString();
            string currencyName = functionCall.GetProperty("args").GetProperty("currencyName").GetString();
            if (string.IsNullOrWhiteSpace(currencyName))
            {
                throw new ArgumentException($"Invalid currency name: {currencyName}");
            }

            Console.WriteLine($"functionName: {functionName}");
            Console.WriteLine($"currencyName: {currencyName}");

            return new FunctionResponse
            {
                FunctionName = functionName,
                CurrencyName = currencyName,
                CurrentData = ""
            };
        }
        public async Task<ApiResponse> GetCoinDetails(string prompt)
        {
            try
            {
                var functionResponse = await GetFunctionResponse(prompt);
                var coinDto = await MakeApiRequest(functionResponse.CurrencyName.ToLower());

                // Serialize coinDto thủ công để kiểm soát định dạng
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                string coinDtoJson = JsonSerializer.Serialize(coinDto, jsonOptions);

                string geminiApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={GEMINI_API_KEY}";
                var requestBody = new
                {
                    contents = new[]
                    {
                        new ContentItem { Role = "user", Parts = new[] { new Part { Text = prompt } } },
                        new ContentItem { Role = "model", Parts = new[] { new Part { FunctionCall = new FunctionCall { Name = "getCoinDetails", Args = new { currencyName = functionResponse.CurrencyName, currencyData = functionResponse.CurrentData } } } } },
                        new ContentItem { Role = "function", Parts = new[] { new Part { FunctionResponse = new FunctionResponseData { Name = "getCoinDetails", Response = new ResponseData { Name = "getCoinDetails", Content = JsonDocument.Parse(coinDtoJson) } } } } }
                    },
                    tools = new[]
                    {
                        new
                        {
                            functionDeclarations = new[]
                            {
                                new
                                {
                                    name = "getCoinDetails",
                                    description = "Get crypto currency data from given currency object.",
                                    parameters = new
                                    {
                                        type = "object",
                                        properties = new
                                        {
                                            currencyName = new
                                            {
                                                type = "string",
                                                description = "The coin ID (e.g., 'bitcoin', 'ethereum')"
                                            },
                                            currencyData = new
                                            {
                                                type = "string",
                                                description = "Additional currency data"
                                            }
                                        },
                                        required = new[] { "currencyName" }
                                    }
                                }
                            }
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody, jsonOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(geminiApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Gemini API error: {errorBody}");
                    throw new HttpRequestException($"Gemini API request failed: {response.StatusCode}, {errorBody}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Gemini API response: {responseBody}");

                var jsonDoc = JsonDocument.Parse(responseBody);
                string text = jsonDoc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text").GetString();

                return new ApiResponse { message = text };
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON serialization error: {ex.Message}");
                throw new Exception("Failed to create request body", ex);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }

        }    
     
        

        public async Task<string> SimpleChat(string prompt)
        {
            string geminiApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={GEMINI_API_KEY}";
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[] { new { text = prompt } }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(geminiApiUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

    public interface IChatboxService
    {
        Task<ApiResponse> GetCoinDetails(string prompt);
        Task<string> SimpleChat(string prompt);
    }
}
