using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.DbContexts;
using RangoAgil.Api.Entities;
using RangoAgil.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"]);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.MapGet("rangos", () =>
//{
//    return "Esta Funcionando Muito Bem";
//});

//app.MapGet("rangos/{numero}/{nome}", (int numero,string nome) =>
//{
//    return $"{nome}-{numero}";
//});

//app.MapGet("/rangos/{numero}", (int numero) =>
//{
//    return $"Numero{numero}";
//});

//app.MapGet("/rangos", async (RangoDbContext rangoDbContext) =>
//{
//    return await rangoDbContext.Rangos.ToListAsync();
//});

//app.MapGet("rango/{nome}", async (RangoDbContext rangoDbContext, string nome) =>
//{
//    return  await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Nome == nome);
//});

app.MapGet("rangos", async Task<Results<NoContent,Ok<IEnumerable<RangoDTO>>>> (RangoDbContext rangoDbContext, [FromQuery(Name = "name")] string? rangonome, IMapper mapper) =>
{
    var rangosEntity = await rangoDbContext.Rangos
                                .Where(x => rangonome == null || x.Nome.ToLower().Contains(rangonome.ToLower()))
                                .ToListAsync();

    if (rangosEntity.Count <= 0 || rangosEntity == null)
        return TypedResults.NoContent();
    else
       return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
});

app.MapGet("rango/{id:int}", async (int id, RangoDbContext rangoDbContext, IMapper mapper) =>
{
    return mapper.Map<RangoDTO>(await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == id));
});


//// [FromQuery] =  http://localhost:5100/rango?id=2
//app.MapGet("/rango", ([FromQuery] int id, RangoDbContext rangoDbContext) =>
//{
//    return rangoDbContext.Rangos.FirstOrDefault(x => x.Id == id);
//});


//// [FromHeader] =  http://localhost:5100/rango + id no header
//app.MapGet("/rango", ([FromHeader] int id, RangoDbContext rangoDbContext) =>
//{
//    return rangoDbContext.Rangos.FirstOrDefault(x => x.Id == id);
//});


// [FromHeader] =  http://localhost:5100/rango + mudando o nome do Id no Header
//app.MapGet("/rango", async ([FromHeader(Name ="RangoId")] int id, RangoDbContext rangoDbContext) =>
//{
//    return await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == id);
//});

app.MapGet("/rango/{rangoid:int}/ingredientes", async (RangoDbContext rangoDbContext, int rangoid,IMapper mapper) =>
{
    return mapper.Map<IEnumerable<IngredienteDTO>>((await rangoDbContext.Rangos
                               .Include(rango => rango.Ingredientes)
                               .FirstOrDefaultAsync(ingre => ingre.Id == rangoid))?.Ingredientes);
});
app.Run();
