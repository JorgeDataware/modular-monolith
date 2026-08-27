using MediatR;
using Microsoft.Extensions.Logging;
using Movies.Module.Contracts.Events;

namespace Users.Module.Application.Integrations;

/// <summary>
/// Reacciona a un cambio de precio publicado por el módulo de películas.
///
/// El carrito guarda solo el MovieId y el precio se resuelve al momento de la compra,
/// así que aquí no hay nada que corregir en base de datos: se deja traza del hecho.
/// El punto arquitectónico es que Movies no conoce a Users; cuando el carrito necesite
/// congelar precios, la lógica se agrega aquí sin tocar el módulo de películas.
/// </summary>
internal sealed class MoviePriceChangedNotificationHandler(
    ILogger<MoviePriceChangedNotificationHandler> logger)
    : INotificationHandler<MoviePriceChangedNotification>
{
    public Task Handle(MoviePriceChangedNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "El precio de la película {MovieId} cambió de {OldPrice} a {NewPrice}; " +
            "los carritos que la contengan cotizarán el precio nuevo.",
            notification.MovieId, notification.OldPrice, notification.NewPrice);

        return Task.CompletedTask;
    }
}
