using Cryptalk.Domain.Chats;
using Cryptalk.Domain.Devices;
using Cryptalk.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cryptalk.Infrastructure.Persistance
{
    public class CryptalkDbContext : DbContext
    {
        public CryptalkDbContext(DbContextOptions<CryptalkDbContext> options)
            :base(options) {}

        public DbSet<Device> Devices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chat>(b =>
            {
                b.HasKey(x => x.ChatId);

                b.Property(x => x.ChatId)
                 .ValueGeneratedOnAdd()
                 .HasAnnotation("Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
            });

            modelBuilder.Entity<Message>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id)
                 .ValueGeneratedOnAdd()
                 .HasAnnotation("Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
                b.HasOne(x => x.Chat)
                 .WithMany(c => c.Messages)
                 .HasForeignKey(x => x.ChatId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(m => new { m.ChatId, m.Timestamp, m.Id });
            });

            modelBuilder.Entity<ChatParticipant>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id)
                 .ValueGeneratedOnAdd()
                 .HasAnnotation("Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
                b.HasOne(x => x.Chat)
                 .WithMany(c => c.Participants)
                 .HasForeignKey(x => x.ChatId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Device>(b =>
            {
                b.HasKey(x => x.DeviceId);
            });
        }
    }
}