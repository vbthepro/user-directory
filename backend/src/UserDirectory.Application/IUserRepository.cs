using UserDirectory.Domain;

namespace UserDirectory.Application;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> ListAsync(CancellationToken cancellationToken);
    Task<User?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task DeleteAsync(User user, CancellationToken cancellationToken);
}
