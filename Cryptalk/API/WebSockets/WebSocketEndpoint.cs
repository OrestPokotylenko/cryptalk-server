using Cryptalk.Infrastructure.Realtime;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Cryptalk.API.WebSockets
{
    public static class WebSocketEndpoint
    {
        public static async Task HandleAsync(HttpContext ctx, ConnectionRegistry reg)
        {
            if (!ctx.WebSockets.IsWebSocketRequest)
            {
                ctx.Response.StatusCode = 400;
                return;
            }

            using var ws = await ctx.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[32 *  1024];
            Guid? connId = null;
            int? chatId = null;

            try
            {
                while (ws.State == WebSocketState.Open && !ctx.RequestAborted.IsCancellationRequested)
                {
                    var res = await ws.ReceiveAsync(buffer, ctx.RequestAborted);

                    if (res.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    var json = Encoding.UTF8.GetString(buffer, 0, res.Count);
                    using var doc = JsonDocument.Parse(json);
                    var type = doc.RootElement.GetProperty("type").GetString();
                    var message = doc.RootElement.GetProperty("payload").GetString();

                    if (type == "join")
                    {
                        chatId = doc.RootElement.GetProperty("chatId").GetInt32();
                        connId = reg.Add(chatId.Value, ws);
                        await ws.SendAsync(Encoding.UTF8.GetBytes("{\"type\":\"ack\"}"),
                            WebSocketMessageType.Text, true, ctx.RequestAborted);
                    }
                    else if (type == "ping")
                    {
                        var pong = JsonSerializer.Serialize(new { type = "pong", payload = message });
                        await ws.SendAsync(Encoding.UTF8.GetBytes(pong),
                            WebSocketMessageType.Text, true, ctx.RequestAborted);
                    }
                }
            }
            finally {
                if (connId.HasValue && chatId.HasValue)
                {
                    reg.Remove(chatId.Value, connId.Value);

                    try
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", ctx.RequestAborted);
                    }
                    catch { }
                }
            }
        }
    }
}