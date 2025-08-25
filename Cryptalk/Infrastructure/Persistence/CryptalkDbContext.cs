using Cryptalk.Domain.Chat;
using Cryptalk.Domain.Device;
using Cryptalk.Models;
using Microsoft.EntityFrameworkCore;

namespace Cryptalk.Infrastructure.Persistence
{
    public class CryptalkDbContext : DbContext
    {
        public CryptalkDbContext(DbContextOptions<CryptalkDbContext> options)
            :base(options) {}

        public DbSet<Device> Devices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}