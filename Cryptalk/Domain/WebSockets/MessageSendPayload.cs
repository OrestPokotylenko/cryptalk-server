namespace Cryptalk.Domain.WebSockets
{
    public record MessageSendPayload(
        string TempId,
        string SenderId,
        string Aad,
        string Ciphertext,
        string Nonce,
        string Tag,
        DateTime Timestamp
    );
}