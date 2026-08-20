namespace CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;

internal record AddMovieRequest(
    string Title,
    string Description,
    DateOnly ReleaseYear,
    int DurationMinutes,
    string Language,
    decimal RentalPrice,
    IEnumerable<Guid> Genres,
    IEnumerable<Participant> Casters,
    IEnumerable<Participant> Crewers
);

internal record Participant
(
    Guid PersonId,
    string role
);