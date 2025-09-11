namespace Cryptalk.Domain.WebSockets
{
    public record WsEnvelope<T>(
        string Type,
        int? ChatId,
        string? RequestId,
        DateTime? Ts,
        T? Payload
    );
}