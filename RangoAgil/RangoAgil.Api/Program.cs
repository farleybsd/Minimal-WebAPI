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

// Route Group
var rangosEndPoint = app.MapGroup("/rangos");
var rangosComIdEndPoint = rangosEndPoint.MapGroup("/{rangoid:int}");
var ingredientesEndPoint = rangosComIdEndPoint.MapGroup("/ingredientes");

rangosEndPoint.MapGet("", async Task<Results<NoContent,Ok<IEnumerable<RangoDTO>>>> (RangoDbContext rangoDbContext, [FromQuery(Name = "name")] string? rangonome, IMapper mapper) =>
{
    var rangosEntity = await rangoDbContext.Rangos
                                .Where(x => rangonome == null || x.Nome.ToLower().Contains(rangonome.ToLower()))
                                .ToListAsync();

    if (rangosEntity.Count <= 0 || rangosEntity == null)
        return TypedResults.NoContent();
    else
       return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
});

rangosComIdEndPoint.MapGet("", async (int id, RangoDbContext rangoDbContext, IMapper mapper) =>
{
    return mapper.Map<RangoDTO>(await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == id));
}).WithName("GetRangos");



ingredientesEndPoint.MapGet("", async (RangoDbContext rangoDbContext, int rangoid,IMapper mapper) =>
{
    return mapper.Map<IEnumerable<IngredienteDTO>>((await rangoDbContext.Rangos
                               .Include(rango => rango.Ingredientes)
                               .FirstOrDefaultAsync(ingre => ingre.Id == rangoid))?.Ingredientes);
});


rangosEndPoint.MapPost("", async Task<CreatedAtRoute<RangoDTO>> (RangoDbContext rangoDbContext,
    IMapper mapper, 
    [FromBody] RangoParaCriacaoDTO rangoParaCriacao
   // LinkGenerator linkGenerator
    //,HttpContext httpContext 
    ) =>
{
    var rangoentity = mapper.Map<Rango>(rangoParaCriacao);
    await rangoDbContext.Rangos.AddAsync(rangoentity);
    await rangoDbContext.SaveChangesAsync();

    var rangoToReturn = mapper.Map<RangoDTO>(rangoentity);
    
    //var linkToReturn = linkGenerator.GetUriByName(httpContext, "GetRango", new { id = rangoToReturn.Id });
    // return TypedResults.Created(linkToReturn, rangoToReturn);

    return TypedResults.CreatedAtRoute(rangoToReturn, "GetRangos", new { rangoid = rangoToReturn.Id });
;});

rangosComIdEndPoint.MapPut("", async Task<Results<NotFound,Ok>> (RangoDbContext rangoDbContext , IMapper mapper, int rangoid, RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO) =>
{
    var rangosEntity = await rangoDbContext.Rangos
                                                .FirstOrDefaultAsync(x => x.Id == rangoid);

    if (rangosEntity == null)
        return TypedResults.NotFound();

  
    var ranngoupdate =mapper.Map(rangoParaAtualizacaoDTO, rangosEntity);
    await rangoDbContext.SaveChangesAsync();

    return TypedResults.Ok();
   
});


rangosComIdEndPoint.MapDelete("", async Task<Results<NotFound, NoContent>> (RangoDbContext rangoDbContext, int rangoid) =>
{
    var rangosEntity = await rangoDbContext.Rangos
                                                .FirstOrDefaultAsync(x => x.Id == rangoid);

    if (rangosEntity == null)
        return TypedResults.NotFound();

    rangoDbContext.Rangos.Remove(rangosEntity);
  
    await rangoDbContext.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.Run();


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
