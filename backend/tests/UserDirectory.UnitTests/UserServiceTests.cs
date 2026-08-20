using UserDirectory.Application;
using UserDirectory.Domain;
using Xunit;

namespace UserDirectory.UnitTests;

public sealed class UserServiceTests
{
    [Fact]
    public async Task CreateAsync_creates_user_and_persists()
    {
        var repo = new FakeRepository();
        var service = new UserService(repo);
        var result = await service.CreateAsync(new UserRequest { Name = "Ada Lovelace", Age = 36, City = "London", State = "London", Pincode = "10001" }, default);
        Assert.Equal("Ada Lovelace", result.Name);
        Assert.Single(repo.Users);
    }

    [Fact]
    public async Task CreateAsync_rejects_invalid_name()
    {
        var service = new UserService(new FakeRepository());
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(new UserRequest { Name = "A", Age = 36, City = "London", State = "London", Pincode = "10001" }, default));
    }

    [Fact]
    public async Task CreateAsync_rejects_missing_age()
    {
        var service = new UserService(new FakeRepository());
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(new UserRequest { Name = "Ada Lovelace", City = "London", State = "London", Pincode = "10001" }, default));
    }

    [Fact]
    public async Task UpdateAsync_returns_null_for_unknown_id()
    {
        var service = new UserService(new FakeRepository());
        var result = await service.UpdateAsync(Guid.NewGuid(), new UserRequest { Name = "Ada Lovelace", Age = 36, City = "London", State = "London", Pincode = "10001" }, default);
        Assert.Null(result);
    }

    private sealed class FakeRepository : IUserRepository
    {
        public List<User> Users { get; } = [];
        public Task AddAsync(User user, CancellationToken ct) { Users.Add(user); return Task.CompletedTask; }
        public Task DeleteAsync(User user, CancellationToken ct) { Users.Remove(user); return Task.CompletedTask; }
        public Task<User?> GetAsync(Guid id, CancellationToken ct) => Task.FromResult(Users.FirstOrDefault(x => x.Id == id));
        public Task<IReadOnlyList<User>> ListAsync(CancellationToken ct) => Task.FromResult<IReadOnlyList<User>>(Users);
        public Task SaveChangesAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
