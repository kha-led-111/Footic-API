
using footic.EData;
using footic.Services;
using Microsoft.EntityFrameworkCore;

namespace footic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //DataBase
            builder.Services.AddDbContext<PlSimulationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.
            builder.Services.AddScoped<LeagueStandingService>();
            builder.Services.AddScoped<TeamService>();
            builder.Services.AddScoped<IplayerService, PlayerService>();
            builder.Services.AddScoped<MatchService>();
            builder.Services.AddScoped<CreateMatchesService>();
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
