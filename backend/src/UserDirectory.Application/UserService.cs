using UserDirectory.Domain;
using UserDirectory.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace UserDirectory.Application;

public sealed class UserService(IUserRepository repository)
{
    public async Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct) =>
        (await repository.ListAsync(ct)).Select(Map).ToList();

    public async Task<UserDto?> GetAsync(Guid id, CancellationToken ct)
    {
        var user = await repository.GetAsync(id, ct);
        return user is null ? null : Map(user);
    }

    public async Task<UserDto> CreateAsync(UserRequest request, CancellationToken ct)
    {
        Validate(request);
        var user = new User(request.Name, request.Age!.Value, request.City, request.State, request.Pincode);
        await repository.AddAsync(user, ct);
        await repository.SaveChangesAsync(ct);
        return Map(user);
    }

    public async Task<UserDto?> UpdateAsync(Guid id, UserRequest request, CancellationToken ct)
    {
        Validate(request);
        var user = await repository.GetAsync(id, ct);
        if (user is null) return null;
        user.Update(request.Name, request.Age!.Value, request.City, request.State, request.Pincode);
        await repository.SaveChangesAsync(ct);
        return Map(user);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var user = await repository.GetAsync(id, ct);
        if (user is null) return false;
        await repository.DeleteAsync(user, ct);
        await repository.SaveChangesAsync(ct);
        return true;
    }

    private static void Validate(UserRequest request)
    {
        var context = new System.ComponentModel.DataAnnotations.ValidationContext(request);
        Validator.ValidateObject(request, context, validateAllProperties: true);
    }

    private static UserDto Map(User user) => new(user.Id, user.Name, user.Age, user.City, user.State, user.Pincode);
}
