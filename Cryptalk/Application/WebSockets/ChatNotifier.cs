using Cryptalk.Infrastructure.Realtime;
using System.Text.Json;

namespace Cryptalk.Application.WebSockets
{
    public class ChatNotifier(ConnectionRegistry reg)
    {
        private readonly ConnectionRegistry _reg = reg;

        public Task MessageCreatedAsync(int chatId, object evt, CancellationToken ct)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(evt);
            return _reg.BroadcastAsync(chatId, bytes, ct);
        }
    }
}