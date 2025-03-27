using Microsoft.AspNetCore.Identity;
using RangoAgil.Api.EndpointFilters;
using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGroup("/identity/").MapIdentityApi<IdentityUser>();

        endpointRouteBuilder.MapGet("/pratos/{pratoid:int}", (int pratoid) => $"O prato {pratoid} é delicioso!")
            .WithOpenApi(operation =>
            {
                operation.Deprecated = true;
                return operation;
            })
            .WithSummary("Este endpoint está deprecated e será descontinuado na versão 2 desta API")
            .WithDescription("Por favor utilize a outra rota desta API sendo ela /rangos/{rangoId} para evitar maiores transtornos futuros");

        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos")
            .RequireAuthorization(); // Autenticacao
        var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}");

        var rangosComIdAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
            .RequireAuthorization("RequireAdminFromBrazil") // Politicas Clains
            .RequireAuthorization() // Autenticacao
            .AddEndpointFilter(new RangoIsLockedFilter(8))
            .AddEndpointFilter(new RangoIsLockedFilter(12));

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync)
            .WithOpenApi()
            .WithSummary("Esta rota retornará todos os Rangos");

        rangosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos")
            .AllowAnonymous(); // Sem Autenticacao

        rangosEndpoints.MapPost("", RangosHandlers.CreateRangoAsync)
            .AddEndpointFilter<ValidateAnnotationFilter>();

        rangosComIdAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);

        rangosComIdAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>();
    }

    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes")
            .RequireAuthorization(); // Autenticacao

        ingredientesEndpoints.MapGet("", IngredientesHandlers.GetIngredientesAsync);
        ingredientesEndpoints.MapPost("", () =>
        {
            throw new NotImplementedException();
        });
    }
    //    endpointRouteBuilder.MapGet("/pratos/{pratoid:int}", (int pratoid) => $"O prato {pratoid} é delicioso!")
    //        .WithOpenApi(operation =>
    //        {
    //            operation.Deprecated = true;
    //            return operation;
    //        })
    //        .WithSummary("Este endpoint está deprecated e será descontinuado na versão 2 desta API")
    //        .WithDescription("Por favor utilize a outra rota desta API sendo ela /rangos/{rangoId} para evitar maiores transtornos futuros");

    //    var rangosEndPoint = endpointRouteBuilder.MapGroup("/rangos");
    //    var rangosComIdEndPoint = rangosEndPoint.MapGroup("/{rangoid:int}");

    //    var rangosComIdAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoid:int}")
    //                                                                 .AddEndpointFilter(new RangoIsLockedFilter(12))
    //                                                                .AddEndpointFilter(new RangoIsLockedFilter(8));

    //        rangosEndPoint.MapGet("", RangosHandlers.GetRangosAsync)
    //       .WithOpenApi()
    //       .WithSummary("Esta rota retornará todos os Rangos");

    //    rangosComIdEndPoint.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");

    //    rangosEndPoint.MapPost("", RangosHandlers.CreateRangoAsync)
    //                    .AddEndpointFilter<ValidateAnnotationFilter>();

    //    rangosComIdAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);
    //    // .AddEndpointFilter<RangoIsLockedFilter>();
    //    //.AddEndpointFilter(new RangoIsLockedFilter(12))
    //    //.AddEndpointFilter(new RangoIsLockedFilter(8));

    //    rangosComIdAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
    //                                      .AddEndpointFilter<LogNotFoundResponseFilter>();
    //    //.AddEndpointFilter<RangoIsLockedFilter>();
    //    // .AddEndpointFilter(new RangoIsLockedFilter(12))
    //    //.AddEndpointFilter(new RangoIsLockedFilter(8));
    //}

    //public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    //{
    //    var ingredientesEndPoint = endpointRouteBuilder.MapGroup("/rangos/{rangoid:int}/ingredientes");
    //    ingredientesEndPoint.MapGet("", IngredientesHandlers.GetIngredientesAsync);
    //}
}