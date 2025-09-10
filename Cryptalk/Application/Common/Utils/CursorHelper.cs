using System.Globalization;

namespace Cryptalk.Application.Common.Utils
{
    public sealed class CursorHelper
    {
        public static string Encode<TId>(DateTime timestampUtc, TId id)
            where TId : IFormattable
            => $"{timestampUtc.ToUniversalTime():O}|{id.ToString(null, CultureInfo.InvariantCulture)}";

        public static (DateTime tsUtc, TId id) Decode<TId>(string cursor)
            where TId : IParsable<TId>
        {
            var parts = cursor.Split('|', 2);
            var ts = DateTime.Parse(parts[0], null, DateTimeStyles.RoundtripKind).ToUniversalTime();
            var id = TId.Parse(parts[1], CultureInfo.InvariantCulture);
            return (ts, id);
        }
    }
}