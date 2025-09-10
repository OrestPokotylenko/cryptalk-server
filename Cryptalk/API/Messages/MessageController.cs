using Cryptalk.Application.Messages;
using Cryptalk.Application.Messages.Dtos;
using Cryptalk.Domain.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Cryptalk.API.Messages
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDto request, CancellationToken ct)
        {
            int messageId = await _messageService.CreateMessageAsync(request, ct);
            return Ok(new { id = messageId });
        }

        [HttpGet("{chatId}/messages")]
        public async Task<ActionResult<object>> GetMessages(int chatId, [FromQuery] string? cursor, [FromQuery] int take = 50, CancellationToken ct = default)
        {
            take = Math.Clamp(take, 1, 200);
            var (items, next) = await _messageService.FetchMessagesBeforeAsync(chatId, cursor, take, ct);

            return Ok(new
            {
                items,
                nextCursor = next
            });
        }

        [HttpGet("{chatId}/messages/new")]
        public async Task<ActionResult<(IReadOnlyList<Message> Items, string? NextCursor)>> FetchMessagesAfterCursorAsync(int chatId, string? afterCursor, int take, CancellationToken ct)
        {
            var (items, next) = await _messageService.FetchMessagesAfterCursorAsync(chatId, afterCursor, take, ct);

            return Ok(new
            {
                items,
                nextCursor = next
            });
        }
    }
}