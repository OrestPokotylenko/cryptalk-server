using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Cryptalk.Infrastructure.Realtime
{
    public class ConnectionRegistry
    {
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

                if (room.IsEmpty)
                {
                    _rooms.TryRemove(chatId, out _);
                }
            }
        }

        public async Task BroadcastAsync(int chatId, ReadOnlyMemory<byte> payload, CancellationToken ct = default)
        {
            if (!_rooms.TryGetValue(chatId, out var room))
            {
                return;
            }

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
                    await ws.SendAsync(payload, WebSocketMessageType.Text, true, ct);
                }
                catch
                {
                    room.TryRemove(kv.Key, out _);
                }
            }
        }
    }
}
