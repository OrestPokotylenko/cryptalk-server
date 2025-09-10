using Cryptalk.Domain.Chats;

namespace Cryptalk.Infrastructure.Persistance.Repositories
{
    public class ChatRepository : EfRepository<Chat>
    {
        public ChatRepository(CryptalkDbContext context)
            :base(context)
        {
            
        }
    }
}