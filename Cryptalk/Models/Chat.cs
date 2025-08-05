using System.ComponentModel.DataAnnotations;

namespace Cryptalk.Models
{
    public class Chat
    {
        [Key]
        public string ChatId { get; set; }   // UUID for group or hash for direct

        [Required]
        public string ChatType { get; set; } // "direct" or "group"

        [Required]
        public string OwnerDeviceId { get; set; } // who created the chat

        public string? GroupPublicKey { get; set; } // only for group chats

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ChatParticipant> Participants { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }
}