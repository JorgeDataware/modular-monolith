using Core.Contracts.Abstractions;
using MediatR;
using Movies.Module.Contracts.Queries;
using Users.Module.Domain;
using Users.Module.Domain.Repositories.CartMovieRepository;
using Users.Module.Utilities.Errors;

namespace Users.Module.Application.Services.CartMovieService;

internal class CartMovieService(ICartMovieRepository repo, ISender sender) : ICartMovieService
{
    private readonly ICartMovieRepository _repo = repo;

    // ISender es la mitad de IMediator que envía requests y espera respuesta.
    // Esta clase no conoce IMovieService ni el ensamblado de películas: solo el mensaje.
    private readonly ISender _sender = sender;

    public async Task<Result<Guid>> AddCartMovieAsync(Guid movieId, string userId, CancellationToken ct)
    {
        if (movieId == Guid.Empty)
            return Result<Guid>.Failure(CartMovieErrors.MovieIdEmpy);

        // Comunicación entre módulos: se le pregunta a Movies si la película existe.
        // Antes de esto el carrito aceptaba cualquier Guid y guardaba basura.
        var movie = await _sender.Send(new GetMovieSummaryQuery(movieId), ct);

        if (!movie.IsSuccess)
            return Result<Guid>.Failure(movie.Error);

        var cartMovie = new CartMovie(userId, movie.Value.Id);

        await _repo.AddCartMovieAsync(cartMovie, ct);

        return Result<Guid>.Success(cartMovie.Id);
    }

    public async Task<Result<Guid>> DeleteCartMovieAsync(Guid movieId, string userId, CancellationToken ct)
    {
        if (movieId == Guid.Empty)
            return Result<Guid>.Failure(CartMovieErrors.MovieIdEmpy);

        var affectedRows = await _repo.DeleteCartMovieAsync(movieId, userId, ct);

        if (affectedRows < 1)
            return Result<Guid>.Failure(CartMovieErrors.MovieNotFound);

        return Result<Guid>.Success(movieId);
    }
}
