using MiniValidation;
using RangoAgil.Api.Models;

namespace RangoAgil.Api.EndpointFilters;

public class ValidateAnnotationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var rangoParaCriacaoDTO = context.GetArgument<RangoParaCriacaoDTO>(2);

        if (!MiniValidator.TryValidate(rangoParaCriacaoDTO, out var validationErrors))
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        return await next(context);
    }
}