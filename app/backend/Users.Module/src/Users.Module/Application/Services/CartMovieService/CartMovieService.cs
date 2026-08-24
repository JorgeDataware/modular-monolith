using Core.Contracts.Abstractions;
using Users.Module.Application.Endpoints.CartMovieEndpoints.AddCartMovie;
using Users.Module.Domain;
using Users.Module.Domain.Repositories.CartMovieRepository;
using Users.Module.Utilities.Errors;

namespace Users.Module.Application.Services.CartMovieService;

internal class CartMovieService(ICartMovieRepository repo) : ICartMovieService
{
    private readonly ICartMovieRepository _repo = repo;

    public async Task<Result<Guid>> AddCartMovieAsync(Guid movieId, string userId, CancellationToken ct)
    {
        if (movieId == Guid.Empty)
            return Result<Guid>.Failure(CartMovieErrors.MovieIdEmpy);

        var cartMovie = new CartMovie(userId, movieId);

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
