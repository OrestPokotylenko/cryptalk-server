
using Cryptalk.Data;
using Microsoft.EntityFrameworkCore;

namespace Cryptalk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CryptalkDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectiongit add ")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            Console.WriteLine("Cryptalk server started");
            app.Run();
        }
    }
}
