using Core.Contracts.Abstractions;
using Users.Module.Application.Endpoints.CartMovieEndpoints.AddCartMovie;

namespace Users.Module.Application.Services.CartMovieService;

internal interface ICartMovieService
{
    Task<Result<Guid>> AddCartMovieAsync(Guid movieId, string userId, CancellationToken ct);
    Task<Result<Guid>> DeleteCartMovieAsync(Guid movieId, string userId, CancellationToken ct);
    //Task<Result<>> GetCartMovieAsync();
}
