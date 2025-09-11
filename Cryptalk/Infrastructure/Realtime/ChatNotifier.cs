using Cryptalk.Domain.WebSockets;
using System.Text.Json;

namespace Cryptalk.Infrastructure.Realtime
{
    public sealed class ChatNotifier(ConnectionRegistry registry)
    {
        private static readonly JsonSerializerOptions Json = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        private readonly ConnectionRegistry _registry = registry;

        // Primary API: publish typed envelopes
        public Task PublishAsync<T>(int chatId, WsEnvelope<T> evt, CancellationToken ct = default)
            => _registry.BroadcastAsync(chatId, JsonSerializer.SerializeToUtf8Bytes(evt, Json), ct);

        // Optional low-level raw JSON
        public Task PublishRawAsync(int chatId, object obj, CancellationToken ct = default)
            => _registry.BroadcastAsync(chatId, JsonSerializer.SerializeToUtf8Bytes(obj, Json), ct);
    }
}