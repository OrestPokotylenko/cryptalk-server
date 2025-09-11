using Cryptalk.API.WebSockets;
using Cryptalk.Application.Chats;
using Cryptalk.Application.Messages;
using Cryptalk.Infrastructure.Persistance;
using Cryptalk.Infrastructure.Persistance.Repositories;
using Cryptalk.Infrastructure.Realtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cryptalk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var cfg = builder.Configuration;

            builder.Services.AddDbContext<CryptalkDbContext>(options =>
                options.UseNpgsql(cfg["ConnectionStrings:Default"]));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });
            });

            builder.Services.AddScoped<MessageRepository>();
            builder.Services.AddScoped<ChatRepository>();
            builder.Services.AddScoped<MessageService>();
            builder.Services.AddScoped<ChatService>();
            builder.Services.AddSingleton<ConnectionRegistry>();
            builder.Services.AddSingleton<ChatNotifier>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseWebSockets();

            app.Map("/ws/dev", async (HttpContext ctx, [FromServices] MessageService svc, [FromServices] ConnectionRegistry reg) =>
            {
                if (!ctx.WebSockets.IsWebSocketRequest) { ctx.Response.StatusCode = 400; return; }
                var ws = await ctx.WebSockets.AcceptWebSocketAsync();
                await WebSocketEndpoint.HandleAsync(ctx, ws, svc, reg);
            });

            app.MapControllers();

            app.Run();
        }
    }
}