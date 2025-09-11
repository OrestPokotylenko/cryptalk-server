using System.Text;
using System.Text.Json;

namespace Cryptalk.API.WebSockets
{
    public static class Ws
    {
        private static readonly JsonSerializerOptions Json = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public static ArraySegment<byte> JsonBytes<T>(T obj) =>
            new(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj, Json)));
    }
}