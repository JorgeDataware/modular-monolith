using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Contracts.Messaging.Behaviors;

/// <summary>
/// Middleware para mensajes: envuelve la ejecución de CADA handler de MediatR.
/// Es el equivalente a un middleware de ASP.NET pero en el pipeline del mediador,
/// y es la razón principal para usar MediatR en lugar de un mediator artesanal:
/// logging, transacciones o validación se escriben una vez y aplican a todo.
/// </summary>
internal sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Mediator → {Request}", requestName);

        try
        {
            // next() invoca al siguiente behavior o, si no hay más, al handler.
            var response = await next(cancellationToken);

            logger.LogInformation(
                "Mediator ← {Request} en {Elapsed} ms", requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, "Mediator ✗ {Request} falló tras {Elapsed} ms", requestName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
