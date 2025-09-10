using Cryptalk.Domain.Chats;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptalk.Domain.Messages
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public int ChatId { get; private set; }

        [ForeignKey("ChatId")]
        public Chat Chat { get; private set; }

        [Required]
        public string SenderId { get; private set; }

        [Required]
        public string Aad { get; private set; }

        [Required]
        public string Ciphertext { get; private set; }

        [Required]
        public string Nonce { get; private set; }

        [Required]
        public string Tag { get; private set; }

        [Required]
        public DateTime Timestamp { get; private set; }

        private Message() { }
        public Message(int chatId, string senderId, string aad, string ciphertext, string nonce, string tag, DateTime timestamp)
        {
            ChatId = chatId;
            SenderId = senderId;
            Aad = aad;
            Ciphertext = ciphertext;
            Nonce = nonce;
            Tag = tag;
            Timestamp = timestamp;
        }
    }
}