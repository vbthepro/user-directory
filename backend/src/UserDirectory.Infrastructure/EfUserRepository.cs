using Microsoft.EntityFrameworkCore;
using UserDirectory.Application;
using UserDirectory.Application.Interfaces;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure;

public sealed class EfUserRepository(AppDbContext db) : IUserRepository
{
    public async Task<IReadOnlyList<User>> ListAsync(CancellationToken ct) => await db.Users.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);
    public Task<User?> GetAsync(Guid id, CancellationToken ct) => db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(User user, CancellationToken ct) => db.Users.AddAsync(user, ct).AsTask();
    public Task DeleteAsync(User user, CancellationToken ct) { db.Users.Remove(user); return Task.CompletedTask; }
    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
