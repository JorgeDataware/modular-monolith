namespace Users.Module.Domain.Repositories.CartMovieRepository;

internal interface ICartMovieRepository
{
    Task AddCartMovieAsync(CartMovie cartMovie, CancellationToken ct);
    Task<int> DeleteCartMovieAsync(Guid movieId, string userId, CancellationToken ct);
    Task<IEnumerable<Guid>> GetMoviesIds(string userId);
}
