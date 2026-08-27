using MediatR;

namespace Movies.Module.Contracts.Events;

/// <summary>
/// Anuncia que el precio de renta de una película cambió.
/// Notification: la publica el dueño del dato (Movies) y la atienden 0..N módulos.
/// Quien la publica no sabe ni le importa quién escucha; si nadie escucha, no pasa nada.
/// </summary>
public sealed record MoviePriceChangedNotification(
    Guid MovieId,
    decimal OldPrice,
    decimal NewPrice) : INotification;
