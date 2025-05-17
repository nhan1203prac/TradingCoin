using System.Text.Json;

namespace Coin_Exchange.Models.Request
{
    public class ResponseData
    {
        public string Name { get; set; }
        public JsonDocument Content { get; set; }
    }
}
