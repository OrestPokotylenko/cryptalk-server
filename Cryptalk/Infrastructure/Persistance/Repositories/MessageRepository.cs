using Cryptalk.Application.Messages.Dtos;
using Cryptalk.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Cryptalk.Application.Common.Utils;

namespace Cryptalk.Infrastructure.Persistance.Repositories
{
    public class MessageRepository : EfRepository<Message>
    {
        public MessageRepository(CryptalkDbContext context)
            : base(context) { }

        public async Task<(IReadOnlyList<ReturnMessageDto> Items, string? NextCursor)>FetchMessagesBeforeAsync(int chatId, string? cursor, int take, CancellationToken ct)
        {
            var query = _dbSet.AsNoTracking()
                              .Where(m => m.ChatId == chatId);

            if (!string.IsNullOrEmpty(cursor))
            {
                var (ts, id) = CursorHelper.Decode<int>(cursor);
                query = query.Where(m => m.Timestamp < ts || (m.Timestamp == ts && m.Id < id));
            }

            var batch = await query
                .OrderByDescending(m => m.Timestamp).ThenByDescending(m => m.Id)
                .Take(take)
                .Select(m => new
                {
                    m.Id,
                    m.Timestamp,
                    Dto = new ReturnMessageDto(
                        m.SenderId,
                        m.ChatId,
                        m.Aad,
                        m.Ciphertext,
                        m.Nonce,
                        m.Tag,
                        m.Timestamp
                    )
                })
                .ToListAsync(ct);


            if (batch.Count == 0)
                return (Array.Empty<ReturnMessageDto>(), null);

            var items = batch.Select(x => x.Dto).Reverse().ToList();

            string? nextCursor = batch.Count == 0
                ? null
                : CursorHelper.Encode(batch.Last().Timestamp, batch.Last().Id);

            return (items, nextCursor);
        }

        public async Task<(IReadOnlyList<Message> Items, string? NextCursor)>FetchMessagesAfterCursorAsync(int chatId, string? afterCursor, int take, CancellationToken ct)
        {
            var query = _dbSet.AsNoTracking().Where(m => m.ChatId == chatId);

            if (!string.IsNullOrEmpty(afterCursor))
            {
                var (ts, id) = CursorHelper.Decode<int>(afterCursor);
                query = query.Where(m => m.Timestamp > ts || (m.Timestamp == ts && m.Id > id));
            }

            var items = await query
                .OrderBy(m => m.Timestamp).ThenBy(m => m.Id)
                .Take(take)
                .ToListAsync(ct);

            string? next = (items.Count == take)
                ? CursorHelper.Encode(items[^1].Timestamp, items[^1].Id)
                : null;

            return (items, next);
        }
    }
}