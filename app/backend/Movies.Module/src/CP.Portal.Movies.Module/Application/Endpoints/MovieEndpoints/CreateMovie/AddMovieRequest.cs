namespace CP.Portal.Movies.Module.Application.Endpoints.Movie.CreateMovie;

internal record AddMovieRequest(
    string Title,
    string Description,
    DateOnly ReleaseYear,
    int DurationMinutes,
    string Language,
    decimal RentalPrice,
    List<Guid> Genres
);
