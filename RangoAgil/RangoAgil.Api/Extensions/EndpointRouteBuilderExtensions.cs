using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var rangosEndPoint = endpointRouteBuilder.MapGroup("/rangos");
        var rangosComIdEndPoint = rangosEndPoint.MapGroup("/{rangoid:int}");

        rangosEndPoint.MapGet("", RangosHandlers.GetRangosAsync);
        rangosComIdEndPoint.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");
        rangosEndPoint.MapPost("", RangosHandlers.CreateRangoAsync);
        rangosEndPoint.MapPut("", RangosHandlers.UpdateRangoAsync);
        rangosEndPoint.MapDelete("", RangosHandlers.DeleteRangoAsync);
    }

    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndPoint = endpointRouteBuilder.MapGroup("/rangos/{rangoid:int}/ingredientes");
        ingredientesEndPoint.MapGet("", IngredientesHandlers.GetIngredientesAsync);
    }
}