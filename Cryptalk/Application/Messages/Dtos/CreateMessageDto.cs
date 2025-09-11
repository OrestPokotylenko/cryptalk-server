namespace Cryptalk.Application.Messages.Dtos
{
    public class CreateMessageDto
    {
        public int ChatId { get; set; }
        public string SenderId { get; set; }
        public string Aad { get; set; }
        public string Ciphertext { get; set; }
        public string Nonce { get; set; }
        public string Tag { get; set; }
        public DateTime Timestamp { get; set; }
    }
}