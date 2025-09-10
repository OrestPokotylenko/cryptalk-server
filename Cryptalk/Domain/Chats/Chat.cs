using Cryptalk.Domain.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptalk.Domain.Chats
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatId { get; set; }

        [Required]
        public string ChatType { get; set; }

        [Required]
        public string OwnerDeviceId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ChatParticipant> Participants { get; set; } = [];
        public List<Message> Messages { get; set; } = [];

        private Chat() { }

        public Chat(string chatType, string ownerDeviceId) 
        {
            ChatType = chatType;
            OwnerDeviceId = ownerDeviceId;
        }
    }
}