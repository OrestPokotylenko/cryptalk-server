using Cryptalk.Application.Common.Dtos;
using Cryptalk.Application.Messages.Dtos;
using Cryptalk.Domain.Messages;
using Cryptalk.Infrastructure.Persistance.Repositories;
using System.Collections.Generic;

namespace Cryptalk.Application.Messages
{
    public class MessageService(MessageRepository messageRepository)
    {
        private readonly MessageRepository _messageRepository = messageRepository;

        public async Task<int> CreateMessageAsync(CreateMessageDto messageRequest, CancellationToken ct)
        {
            Message message = new(messageRequest.ChatId, 
                messageRequest.SenderId, 
                messageRequest.Aad, 
                messageRequest.Ciphertext, 
                messageRequest.Nonce, 
                messageRequest.Tag, 
                messageRequest.Timestamp);

            await _messageRepository.AddAsync(message, ct);
            await _messageRepository.SaveChangesAsync(ct);

            return message.Id;
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