namespace Movies.Module.Contracts.Dtos;

/// <summary>
/// Vista mínima de una película para consumo de otros módulos.
/// Es un DTO propio del contrato, NO la entidad Movie: si mañana la entidad cambia,
/// los consumidores no se rompen. Ese desacople es justo el objetivo del contrato.
/// </summary>
public sealed record MovieSummary(Guid Id, string Title, decimal RentalPrice);
