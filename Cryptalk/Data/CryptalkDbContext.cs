using Cryptalk.Models;
using Microsoft.EntityFrameworkCore;

namespace Cryptalk.Data
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