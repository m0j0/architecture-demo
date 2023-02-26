using ArchitectureDemo.Models;
using ArchitectureDemo.Services;
using ArchitectureDemo.ValueObjects;
using ArchitectureDemo.WebApi.Host.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureDemo.WebApi.Host.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet("getById")]
    public async Task<ActionResult<GetUserResponse>> Create(Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _usersService.GetUser(new UserId(id), cancellationToken);
        return new GetUserResponse { User = user?.ToDto() };
    }

    [HttpPost("create")]
    public async Task<ActionResult<CreateUserResponse>> Create(CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _usersService.CreateUser(new CreateUserModel(request.Name, request.ParentId), cancellationToken);
        return result.Match(
            userCreated => new CreateUserResponse { ResponseTag = CreateUserResponse.Tag.UserCreated, UserId = userCreated.Id.Value },
            emailAlreadyRegistered => new CreateUserResponse { ResponseTag = CreateUserResponse.Tag.EmailAlreadyRegistered }
        );
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<GetAllUsersResponse>> GetAll(CancellationToken cancellationToken) // добавить пагинацию
    {
        var result = (await _usersService.GetUsers(cancellationToken)).Select(u => u.ToDto());
        return Ok(new GetAllUsersResponse(result));
    }

    [HttpGet("getTree")]
    public async Task<ActionResult<GetUsersTreeResponse>> GetTree(CancellationToken cancellationToken)
    {
        var result = (await _usersService.GetUserTree(cancellationToken)).Select(u => u.ToDto());
        return Ok(new GetUsersTreeResponse(result));
    }
}
