using Microsoft.EntityFrameworkCore;
using prs_serverside_2022.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var AppAccess = "_AppAccess";

builder.Services.AddControllers();

var connStrKey = "AppDbContext";

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString(connStrKey));
});

builder.Services.AddCors(x =>
{
    x.AddPolicy(name: AppAccess,
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseCors(AppAccess);

app.UseAuthorization();

app.MapControllers();

app.Run();

