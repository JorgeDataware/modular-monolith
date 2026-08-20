namespace CP.Portal.Movies.Module.Application.Endpoints.MovieEndpoints.GetMovieByIdAsync;

internal record GetMovieDetailByIdDto
(
    Guid Id,
    string Title,
    string Description,
    DateOnly ReleaseYear,
    int DurationMinutes,
    string Language,
    decimal RentalPrice,
    DateTime CreatedAt,
    IEnumerable<ParticipanDto> Cast,
    IEnumerable<ParticipanDto> Crew,
    IEnumerable<GenreDto> Genres
);

internal record ParticipanDto
(
    Guid PersonId,
    string FullName,
    string Role
);

internal record GenreDto
(
    Guid GenreId,
    string Name
);

/// <summary>
/// Fila plana de la pelicula. ReleaseYear se declara como DateTime porque
/// Microsoft.Data.SqlClient reporta las columnas "date" como System.DateTime,
/// y Dapper resuelve el constructor a partir de los tipos del reader.
/// </summary>
internal record MovieDetailRow
(
    Guid Id,
    string Title,
    string Description,
    DateTime ReleaseYear,
    int DurationMinutes,
    string Language,
    decimal RentalPrice,
    DateTime CreatedAt
);