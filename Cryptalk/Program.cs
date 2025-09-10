using Cryptalk.Application.Chats;
using Cryptalk.Application.Messages;
using Cryptalk.Infrastructure.Persistance;
using Cryptalk.Infrastructure.Persistance.Repositories;
using Cryptalk.Infrastructure.Realtime;
using Microsoft.EntityFrameworkCore;
using Cryptalk.API.WebSockets;

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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseWebSockets();

            app.Map("/ws/dev", (HttpContext ctx, ConnectionRegistry reg) =>
                WebSocketEndpoint.HandleAsync(ctx, reg));

            app.MapControllers();

            app.Run();
        }
    }
}