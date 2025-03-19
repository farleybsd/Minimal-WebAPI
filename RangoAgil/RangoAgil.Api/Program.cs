using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"]);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
