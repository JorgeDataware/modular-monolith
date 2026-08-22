using Core.Contracts.Abstractions;
using Users.Module.Application.Endpoints.CartMovieEndpoints.AddCartMovie;
using Users.Module.Utilities.Errors;

namespace Users.Module.Application.Services.CartMovieService;

internal class CartMovieService : ICartMovieService
{
    public async Task<Result<Guid>> AddCartMovieAsync(Guid movieId, string userId, CancellationToken ct)
    {
        if (movieId == Guid.Empty)
            return Result<Guid>.Failure(CartMovieErrors.MovieIdEmpy);


    }

    public Task<Result<Guid>> DeleteCartMovieAsync(Guid movieId, string userId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
