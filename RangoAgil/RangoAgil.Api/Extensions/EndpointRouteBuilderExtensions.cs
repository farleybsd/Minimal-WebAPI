using RangoAgil.Api.EndpointFilters;
using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var rangosEndPoint = endpointRouteBuilder.MapGroup("/rangos");
        var rangosComIdEndPoint = rangosEndPoint.MapGroup("/{rangoid:int}");

        var rangosComIdAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoid:int}")
                                                                     .AddEndpointFilter(new RangoIsLockedFilter(12))
                                                                    .AddEndpointFilter(new RangoIsLockedFilter(8));

        rangosEndPoint.MapGet("", RangosHandlers.GetRangosAsync);

        rangosComIdEndPoint.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");

        rangosEndPoint.MapPost("", RangosHandlers.CreateRangoAsync)
                        .AddEndpointFilter<ValidateAnnotationFilter>();

        rangosComIdAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);
        // .AddEndpointFilter<RangoIsLockedFilter>();
        //.AddEndpointFilter(new RangoIsLockedFilter(12))
        //.AddEndpointFilter(new RangoIsLockedFilter(8));

        rangosComIdAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
                                          .AddEndpointFilter<LogNotFoundResponseFilter>();
        //.AddEndpointFilter<RangoIsLockedFilter>();
        // .AddEndpointFilter(new RangoIsLockedFilter(12))
        //.AddEndpointFilter(new RangoIsLockedFilter(8));
    }

    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndPoint = endpointRouteBuilder.MapGroup("/rangos/{rangoid:int}/ingredientes");
        ingredientesEndPoint.MapGet("", IngredientesHandlers.GetIngredientesAsync);
    }
}