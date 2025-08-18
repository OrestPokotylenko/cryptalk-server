using Cryptalk.Data;
using Cryptalk.Utils;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Cryptalk
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            // 2. Build AFTER everything is registered
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            string? publicIp = await IpDefiner.GetPublicIpAddressAsync();

            if (publicIp is not null)
            {
                Console.WriteLine($"Your public ip address: {publicIp}");
            }

            app.Run();
        }
    }
}
