using Core.Contracts.Abstractions;
using MediatR;
using Movies.Module.Contracts.Dtos;

namespace Movies.Module.Contracts.Queries;

/// <summary>
/// Pregunta al módulo de películas por los datos básicos de una película.
/// Request/Response: exactamente un handler la atiende y devuelve un valor.
/// Devuelve Result<T>; en lugar de lanzar excepciones, para que el fallo
/// "no existe" sea un dato esperado al cruzar el límite entre módulos.
/// </summary>
public sealed record GetMovieSummaryQuery(Guid MovieId) : IRequest<Result<MovieSummary>>;
