using LoggerService;
using Microsoft.EntityFrameworkCore;
using NLog;
using prs_serverside_2022.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var AppAccess = "_AppAccess";
var connStrKey = "AppDbContext";

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
services.ConfigureLoggerService();

services.AddControllers();
services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString(connStrKey));
});

services.AddCors(x =>
{
    x.AddPolicy(name: AppAccess,
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors(AppAccess);
app.UseAuthorization();
app.MapControllers();

app.Run();

