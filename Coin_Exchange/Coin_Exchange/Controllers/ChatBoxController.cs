using Coin_Exchange.Models.Request;
using Coin_Exchange.Models.Response;
using Coin_Exchange.Service;
using Microsoft.AspNetCore.Mvc;

namespace Coin_Exchange.Controllers
{
    [ApiController]
    [Route("ai/chat")]
    public class ChatBoxController: ControllerBase
    {
        private readonly IChatboxService _chatboxService;

        public ChatBoxController(IChatboxService chatboxService)
        {
            _chatboxService = chatboxService;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> GetCoinDetails([FromBody] PromptBody promptBody)
        {
            var apiResponse = await _chatboxService.GetCoinDetails(promptBody.Prompt);
            return Ok(apiResponse);
        }

        [HttpPost("simple")]
        public async Task<ActionResult<ApiResponse>> SimpleChat([FromBody] PromptBody promptBody)
        {
            var result = await _chatboxService.SimpleChat(promptBody.Prompt);
            return Ok(result);
        }

    }
}
