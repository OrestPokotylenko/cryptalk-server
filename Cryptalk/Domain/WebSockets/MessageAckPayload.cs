namespace Cryptalk.Domain.WebSockets
{
    public record MessageAckPayload(string TempId, int Id);
}