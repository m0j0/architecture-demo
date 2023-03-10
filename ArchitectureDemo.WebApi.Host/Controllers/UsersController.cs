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
    public async Task<ActionResult<GetUserResponse>> GetById(int id,
        CancellationToken cancellationToken)
    {
        var user = await _usersService.GetUser(new UserId(id), cancellationToken);
        return new GetUserResponse { User = user?.ToDto() };
    }

    [HttpPost("create")]
    public async Task<ActionResult<CreateUserResponse>> Create(CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var createUserModel = new CreateUserModel(Name: request.Name,
            Email: request.Email,
            ParentId: request.ParentId.HasValue ? new UserId(request.ParentId!.Value) : null);

        var result = await _usersService.CreateUser(createUserModel, cancellationToken);
        return result.Match(
            userCreated => new CreateUserResponse { ResponseTag = CreateUserResponse.Tag.UserCreated, UserId = userCreated.Id.Value },
            emailAlreadyRegistered => new CreateUserResponse { ResponseTag = CreateUserResponse.Tag.EmailAlreadyRegistered },
            parentNotFound => new CreateUserResponse { ResponseTag = CreateUserResponse.Tag.ParentNotFound }
        );
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<GetAllUsersResponse>> GetAll(CancellationToken cancellationToken) // 
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
