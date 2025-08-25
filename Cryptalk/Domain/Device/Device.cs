using System.ComponentModel.DataAnnotations;

namespace Cryptalk.Domain.Device
{
    public class Device
    {
        [Key]
        public string DeviceId { get; set; }

        [Required]
        public string PublicKey { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}