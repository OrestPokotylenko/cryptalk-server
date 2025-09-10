namespace Cryptalk.Application.Messages.Dtos
{
    public class ReturnMessageDto
    {
        public string SenderId { get; set; }
        public int ChatId { get; set; }
        public string Aad { get; set; }
        public string CipherText {  get; set; }
        public string Nonce {  get; set; }
        public string Tag { get; set; }
        public DateTime Timestamp { get; set; }

        public ReturnMessageDto(string senderId, int chatId, string aad, string cipherText, string nonce, string tag, DateTime timestamp)
        {
            SenderId = senderId;
            ChatId = chatId;
            Aad = aad;
            CipherText = cipherText;
            Nonce = nonce;
            Tag = tag;
            Timestamp = timestamp;
        }
    }
}