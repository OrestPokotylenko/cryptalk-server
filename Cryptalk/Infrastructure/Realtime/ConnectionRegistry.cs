using Cryptalk.Domain.WebSockets;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;

namespace Cryptalk.Infrastructure.Realtime
{
    public sealed class ConnectionRegistry
    {
        private static readonly JsonSerializerOptions Json = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        // chatId -> (connId -> socket)
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<Guid, WebSocket>> _rooms = new();

        public Guid Add(int chatId, WebSocket ws)
        {
            var id = Guid.NewGuid();
            var room = _rooms.GetOrAdd(chatId, _ => new());
            room[id] = ws;
            return id;
        }

        public void Remove(int chatId, Guid id)
        {
            if (_rooms.TryGetValue(chatId, out var room))
            {
                room.TryRemove(id, out _);
                if (room.IsEmpty) _rooms.TryRemove(chatId, out _);
            }
        }

        public async Task BroadcastAsync(int chatId, ReadOnlyMemory<byte> payload, CancellationToken ct = default)
        {
            if (!_rooms.TryGetValue(chatId, out var room)) return;

            foreach (var kv in room.ToArray())
            {
                var ws = kv.Value;
                if (ws.State != WebSocketState.Open)
                {
                    room.TryRemove(kv.Key, out _);
                    continue;
                }

                try
                {
                    await ws.SendAsync(payload, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: ct);
                }
                catch
                {
                    // Drop dead connection
                    room.TryRemove(kv.Key, out _);
                }
            }
        }

        public Task BroadcastAsync<T>(int chatId, WsEnvelope<T> envelope, CancellationToken ct = default)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(envelope, Json);
            return BroadcastAsync(chatId, bytes, ct);
        }
    }
}