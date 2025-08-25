using Cryptalk.Domain.Chat;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptalk.Models
{
    public class Message
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public string ChatId { get; private set; }

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
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
    }
}