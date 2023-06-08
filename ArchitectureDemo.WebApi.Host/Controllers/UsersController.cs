using ArchitectureDemo.Models;
using ArchitectureDemo.Results;
using ArchitectureDemo.Services;
using ArchitectureDemo.ValueObjects;
using ArchitectureDemo.WebApi.Host.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

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
        return new GetUserResponse { User = user == null ? null : ToDto(user) };
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<GetAllUsersResponse>> GetAll(
        [FromQuery] GetAllUsersFilterDto filter,
        CancellationToken cancellationToken)
    {
        var convertedFilter = new GetAllUsersFilter(filter.Name, filter.CreatedAfter);
        var users = await _usersService.GetUsers(convertedFilter, cancellationToken);
        return Ok(new GetAllUsersResponse(users.Select(ToDto)));
    }

    [HttpGet("getTree")]
    public async Task<ActionResult<GetUsersTreeResponse>> GetTree(
        CancellationToken cancellationToken)
    {
        var tree = await _usersService.GetUserTree(cancellationToken);
        return Ok(new GetUsersTreeResponse(tree.Select(ToDto)));
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateUserResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(CreateUserErrorResponse))]
    public async Task<ActionResult<CreateUserResponse>> Create(CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var createUserModel = new CreateUserModel(Name: request.Name,
            Email: request.Email,
            ParentId: request.ParentId.HasValue ? new UserId(request.ParentId!.Value) : null);

        var result = await _usersService.CreateUser(createUserModel, cancellationToken);
        return result switch
        {
            CreateUserResult.Created created =>
                Ok(new CreateUserResponse(created.Id.Value)),
            CreateUserResult.EmailAlreadyRegistered =>
                Conflict(new CreateUserErrorResponse(CreateUserErrorCode.EmailAlreadyRegistered)),
            CreateUserResult.ParentNotFound =>
                Conflict(new CreateUserErrorResponse(CreateUserErrorCode.ParentNotFound)),
            _ => throw new SwitchExpressionException()
        };
    }

    private static UserDto ToDto(UserModel user)
    {
        return new UserDto(user.Id.Value, user.Name, user.FilesCount, user.ParentId?.Value, user.ParentName);
    }

    private static UserWithChildrenDto ToDto(UserWithChildrenModel user)
    {
        return new UserWithChildrenDto(user.Id.Value, user.Name, user.Children.Select(ToDto).ToArray());
    }
}
