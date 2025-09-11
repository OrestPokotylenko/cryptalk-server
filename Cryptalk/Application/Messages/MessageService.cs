using Cryptalk.Application.Common.Dtos;
using Cryptalk.Application.Messages.Dtos;
using Cryptalk.Domain.Messages;
using Cryptalk.Domain.WebSockets;
using Cryptalk.Infrastructure.Persistance.Repositories;
using Cryptalk.Infrastructure.Realtime;

namespace Cryptalk.Application.Messages
{
    public class MessageService(MessageRepository messageRepository, ChatNotifier chatNotifier)
    {
        private readonly MessageRepository _messageRepository = messageRepository;
        private readonly ChatNotifier _chatNotifier = chatNotifier;

        public async Task<int> CreateMessageAsync(CreateMessageDto dto, CancellationToken ct)
        {
            var message = new Message(
                dto.ChatId,
                dto.SenderId,
                dto.Aad,
                dto.Ciphertext,
                dto.Nonce,
                dto.Tag,
                dto.Timestamp
            );

            await _messageRepository.CreateMessageAsync(message, ct);
            await NotifyCreatedAsync(message, ct);
            return message.Id;
        }

        private Task NotifyCreatedAsync(Message message, CancellationToken ct)
        {
            var payload = new
            {
                id = message.Id,
                chatId = message.ChatId,
                message.SenderId,
                message.Aad,
                message.Ciphertext,
                message.Nonce,
                message.Tag,
                message.Timestamp
            };

            var envelope = new WsEnvelope<object>(
                Type: WsTypes.MessageCreated,
                ChatId: message.ChatId,
                RequestId: null,
                Ts: DateTime.UtcNow,
                Payload: payload
            );

            return _chatNotifier.PublishAsync(message.ChatId, envelope, ct);
        }

        public async Task<Message?> GetMessageByIdAsync(int id, CancellationToken ct)
        {
            return await _messageRepository.GetByIdAsync(id, ct);
        }

        public void DeleteMessage(Message message)
        {
            _messageRepository.Delete(message);
        }

        public async Task<(IReadOnlyList<ReturnMessageDto> Items, string? NextCursor)> FetchMessagesBeforeAsync(int chatId, string? cursor, int take, CancellationToken ct)
        {
            var (items, nextCursor) = await _messageRepository.FetchMessagesBeforeAsync(chatId, cursor, take, ct);
            return (items, nextCursor);
        }

        public async Task<(IReadOnlyList<Message> Items, string? NextCursor)>FetchMessagesAfterCursorAsync(int chatId, string? afterCursor, int take, CancellationToken ct)
        {
            var (Items, NextCursor) = await _messageRepository.FetchMessagesAfterCursorAsync(chatId, afterCursor, take, ct);
            return (Items, NextCursor);
        }
    }
}