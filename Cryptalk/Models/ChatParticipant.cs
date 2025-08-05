using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptalk.Models
{
    public class ChatParticipant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ChatId { get; set; }

        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }

        [Required]
        public string DeviceId { get; set; }
    }
}