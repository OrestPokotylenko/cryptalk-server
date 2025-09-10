namespace Cryptalk.Application.Common.Dtos
{
    public record CursorPage<T>(IReadOnlyList<T> Items, string? NextCursor);
}
