using Microsoft.EntityFrameworkCore;
using Users.Module.Infrastructure;

namespace Users.Module.Domain.Repositories.CartMovieRepository;

internal class CartMovieRepository(UsersDbContext usersContext, IUsersConnectionFactory usersConnectionFactory) : ICartMovieRepository
{
    private readonly UsersDbContext _usersContext = usersContext;
    private readonly IUsersConnectionFactory _usersConnectionFactory = usersConnectionFactory;

    // AddAsync solo marca la entidad en el ChangeTracker; sin SaveChangesAsync la fila
    // nunca llegaba a la base. Se persiste aquí para quedar simétrico con
    // DeleteCartMovieAsync, que con ExecuteDeleteAsync también escribe de inmediato.
    public async Task AddCartMovieAsync(CartMovie cartMovie, CancellationToken ct)
    {
        await _usersContext.CartMovie.AddAsync(cartMovie, ct);
        await _usersContext.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteCartMovieAsync(Guid movieId, string userId, CancellationToken ct)
        => await _usersContext.CartMovie.Where(cm => cm.MovieId == movieId && cm.UserId == userId).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<Guid>> GetMoviesIds(string userId)
        => await (from cm in _usersContext.CartMovie.AsNoTracking()
                  where cm.UserId == userId
                  select cm.MovieId).ToListAsync();
}
