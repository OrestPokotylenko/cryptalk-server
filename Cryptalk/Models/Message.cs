using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptalk.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ChatId { get; set; }

        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string Ciphertext { get; set; }

        [Required]
        public string Nonce { get; set; }

        [Required]
        public string Signature { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}