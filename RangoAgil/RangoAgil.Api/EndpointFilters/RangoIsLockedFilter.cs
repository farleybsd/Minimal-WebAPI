namespace RangoAgil.Api.EndpointFilters;

public class RangoIsLockedFilter : IEndpointFilter
{
    private readonly int _lockedRangoId;

    public RangoIsLockedFilter(int lockedRangoId)
    {
        _lockedRangoId = lockedRangoId;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        int rangoId;

        if (context.HttpContext.Request.Method == "PUT")
        {
            rangoId = context.GetArgument<int>(2); // Pega dos Parametros do controller
        }
        else if (context.HttpContext.Request.Method == "DELETE")
        {
            rangoId = context.GetArgument<int>(1); // Pega dos Parametros do controller
        }
        else
        {
            throw new NotSupportedException("Cenarios Nao Suportado");
        }

        if (rangoId == _lockedRangoId)
        {
            return TypedResults.Problem(new()
            {
                Status = 400,
                Title = "Rango ja e perfeito, voce nao precisa modificar ou Deletar nada aqui.",
                Detail = "Voce nao pode modificar ou Deletar esta receita"
            });
        }

        var result = await next.Invoke(context);
        return result;
    }
}