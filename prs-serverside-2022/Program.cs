using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using prs_serverside_2022.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var AppAccess = "_AppAccess";
var connStrKey = "AppDbContext";
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();


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

builder.Logging.ClearProviders();
builder.Host.UseNLog();
var app = builder.Build();

app.UseCors(AppAccess);
app.UseAuthorization();
app.MapControllers();

app.Run();

