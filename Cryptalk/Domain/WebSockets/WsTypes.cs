namespace Cryptalk.Domain.WebSockets
{
    public static class WsTypes
    {
        public const string Join = "join";
        public const string Ping = "ping";
        public const string HistoryBefore = "history_before";
        public const string HistoryAfter = "history_after";

        public const string Ack = "ack";
        public const string Pong = "pong";
        public const string History = "history";
        public const string HistoryAfterResp = "history_after";
        public const string MessageCreated = "message_created";
        public const string Error = "error";
    }
}