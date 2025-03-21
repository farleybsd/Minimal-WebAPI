using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.DbContexts;
using RangoAgil.Api.Models;

namespace RangoAgil.Api.EndPointHandler;

public static class RangoHandlers
{
    public static async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> GetRangosAsync(RangoDbContext rangoDbContext, [FromQuery(Name = "name")] string? rangonome, IMapper mapper)
    {
        var rangosEntity = await rangoDbContext.Rangos
                                    .Where(x => rangonome == null || x.Nome.ToLower().Contains(rangonome.ToLower()))
                                    .ToListAsync();

        if (rangosEntity.Count <= 0 || rangosEntity == null)
            return TypedResults.NoContent();
        else
            return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
    }
}

