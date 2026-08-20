using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UserDirectory.Application;

namespace UserDirectory.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(UserService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> List(CancellationToken ct) =>
        Ok(await service.ListAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> Get(Guid id, CancellationToken ct)
    {
        var user = await service.GetAsync(id, ct);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(UserRequest request, CancellationToken ct)
    {
        try
        {
            var user = await service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }
        catch (ValidationException ex)
        {
            return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]> { ["request"] = [ex.Message] }));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDto>> Update(Guid id, UserRequest request, CancellationToken ct)
    {
        try
        {
            var user = await service.UpdateAsync(id, request, ct);
            return user is null ? NotFound() : Ok(user);
        }
        catch (ValidationException ex)
        {
            return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]> { ["request"] = [ex.Message] }));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct) =>
        await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
