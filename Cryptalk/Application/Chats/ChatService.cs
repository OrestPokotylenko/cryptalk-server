using Cryptalk.Application.Chats.Dtos;
using Cryptalk.Infrastructure.Persistance.Repositories;
using Cryptalk.Domain.Chats;

namespace Cryptalk.Application.Chats
{
    public class ChatService(ChatRepository chatRepository)
    {
        private readonly ChatRepository _chatRepository = chatRepository;

        public async Task<int> CreateChatAsync(CreateChatDto chatRequest, CancellationToken ct)
        {
            Chat chat = new(chatRequest.ChatType, chatRequest.OwnerDeviceId);

            await _chatRepository.AddAsync(chat, ct);
            await _chatRepository.SaveChangesAsync(ct);

            return chat.ChatId;
        }
    }
}