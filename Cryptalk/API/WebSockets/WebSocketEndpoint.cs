using Cryptalk.Application.Messages;
using Cryptalk.Domain.WebSockets;
using Cryptalk.Infrastructure.Realtime;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Cryptalk.API.WebSockets
{
    public static class WebSocketEndpoint
    {
        private static readonly JsonSerializerOptions Json = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public static async Task HandleAsync(HttpContext ctx, WebSocket ws, MessageService messages, ConnectionRegistry reg)
        {
            var buffer = new byte[32 * 1024];
            int? chatId = null;
            Guid? connId = null;

            try
            {
                while (ws.State == WebSocketState.Open && !ctx.RequestAborted.IsCancellationRequested)
                {
                    var res = await ws.ReceiveAsync(buffer, ctx.RequestAborted);
                    if (res.MessageType == WebSocketMessageType.Close) break;

                    var json = Encoding.UTF8.GetString(buffer, 0, res.Count);
                    if (!TryParse(json, out var root))
                    {
                        await Send(ws, new { type = WsTypes.Error, payload = new ErrorPayload("bad_json", "Invalid JSON") }, ctx);
                        continue;
                    }

                    var type = root.TryGetProperty("type", out var tEl) ? tEl.GetString() : null;
                    if (string.IsNullOrWhiteSpace(type))
                    {
                        await Send(ws, new { type = WsTypes.Error, payload = new ErrorPayload("missing_type", "Field 'type' is required") }, ctx);
                        continue;
                    }

                    switch (type)
                    {
                        case WsTypes.Join:
                            {
                                if (!root.TryGetProperty("chatId", out var cidEl) || !cidEl.TryGetInt32(out var cid))
                                {
                                    await Send(ws, new { type = WsTypes.Error, payload = new ErrorPayload("chat_required", "chatId required") }, ctx);
                                    break;
                                }

                                if (connId.HasValue && chatId.HasValue && chatId.Value != cid)
                                {
                                    reg.Remove(chatId.Value, connId.Value);
                                }

                                chatId = cid;
                                connId = reg.Add(chatId.Value, ws);
                                await Send(ws, new { type = WsTypes.Ack }, ctx);
                                break;
                            }

                        case WsTypes.Ping:
                            {
                                string? payload = root.TryGetProperty("payload", out var p) ? p.GetPropertyOrDefault<string?>("nonce") : null;
                                await Send(ws, new { type = WsTypes.Pong, payload }, ctx);
                                break;
                            }

                        case WsTypes.HistoryBefore:
                            {
                                if (!TryGetHistoryParams(root, out var cid, out var take, out var cursor))
                                {
                                    await Send(ws, new { type = WsTypes.Error, payload = new ErrorPayload("chat_required", "chatId required") }, ctx);
                                    break;
                                }

                                var (items, next) = await messages.FetchMessagesBeforeAsync(cid, cursor, take, ctx.RequestAborted);
                                await Send(ws, new { type = WsTypes.History, payload = new { items, nextCursor = next } }, ctx);
                                break;
                            }

                        case WsTypes.HistoryAfter:
                            {
                                if (!TryGetHistoryParams(root, out var cid, out var take, out var cursor))
                                {
                                    await Send(ws, new { type = WsTypes.Error, payload = new ErrorPayload("chat_required", "chatId required") }, ctx);
                                    break;
                                }

                                var (items, next) = await messages.FetchMessagesAfterCursorAsync(cid, cursor, take, ctx.RequestAborted);
                                await Send(ws, new { type = WsTypes.HistoryAfterResp, payload = new { items, nextCursor = next } }, ctx);
                                break;
                            }

                        default:
                            await Send(ws, new { type = WsTypes.Error, payload = new ErrorPayload("unknown_type", $"Unknown type '{type}'") }, ctx);
                            break;
                    }
                }
            }
            finally
            {
                if (connId.HasValue && chatId.HasValue)
                    reg.Remove(chatId.Value, connId.Value);

                if (ws.State is WebSocketState.Open or WebSocketState.CloseReceived)
                {
                    try { await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", ctx.RequestAborted); }
                    catch { /* ignore */ }
                }
                ws.Dispose();
            }
        }

        private static async Task Send(WebSocket ws, object obj, HttpContext ctx)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj, Json));
            await ws.SendAsync(bytes, WebSocketMessageType.Text, endOfMessage: true, ctx.RequestAborted);
        }

        private static bool TryParse(string json, out JsonElement root)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                root = doc.RootElement.Clone();
                return true;
            }
            catch
            {
                root = default;
                return false;
            }
        }

        private static bool TryGetHistoryParams(JsonElement root, out int chatId, out int take, out string? cursor)
        {
            chatId = default;
            take = root.TryGetProperty("take", out var tProp) && tProp.TryGetInt32(out var t) ? Math.Clamp(t, 1, 200) : 50;
            cursor = root.TryGetProperty("cursor", out var cProp) ? cProp.GetString() : null;

            if (!root.TryGetProperty("chatId", out var chatProp) || !chatProp.TryGetInt32(out var cid))
                return false;

            chatId = cid;
            return true;
        }

        private static T? GetPropertyOrDefault<T>(this JsonElement el, string name)
        {
            if (!el.TryGetProperty(name, out var p)) return default;
            try { return JsonSerializer.Deserialize<T>(p.GetRawText(), Json); }
            catch { return default; }
        }
    }
}