using Cryptalk.Application.Chats;
using Cryptalk.Application.Chats.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Cryptalk.API.Chats
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDto request, CancellationToken ct)
        {
            int chatId = await _chatService.CreateChatAsync(request, ct);
            return Ok(new { id = chatId });
        }
    }
}