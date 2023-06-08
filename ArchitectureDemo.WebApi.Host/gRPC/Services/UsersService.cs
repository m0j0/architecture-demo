using System.Runtime.CompilerServices;
using ArchitectureDemo.Models;
using ArchitectureDemo.Services;
using ArchitectureDemo.ValueObjects;
using ArchitectureDemo.gRPC.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ArchitectureDemo.Results;

namespace ArchitectureDemo.WebApi.Host.gRPC.Services;

public class UsersService : Users.UsersBase
{
    private readonly IUsersService _usersService;

    public UsersService(IUsersService usersService)
    {
        _usersService = usersService;
    }
    
    public override async Task<GetByIdResponse> GetById(GetByIdRequest request,
        ServerCallContext context)
    {
        var user = await _usersService.GetUser(new UserId(request.Id), context.CancellationToken);

        return new GetByIdResponse
        {
            User = user == null
                ? null
                : ToDto(user)
        };
    }

    public override async Task<GetAllResponse> GetAll(GetAllFilter request,
        ServerCallContext context)
    {
        var filter = new GetAllUsersFilter(request.Name, request.CreatedAfter?.ToDateTimeOffset());
        var result = await _usersService.GetUsers(filter, context.CancellationToken);
        return new GetAllResponse { Users = { result.Select(ToDto) } };
    }

    public override async Task<GetTreeResponse> GetTree(Empty request, ServerCallContext context)
    {
        var result = (await _usersService.GetUserTree(context.CancellationToken)).Select(ToDto);
        return new GetTreeResponse { Users = { result } };
    }

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        var createUserModel = new CreateUserModel(Name: request.Name,
            Email: request.Email,
            ParentId: request.ParentId.HasValue ? new UserId(request.ParentId!.Value) : null);

        var result = await _usersService.CreateUser(createUserModel, context.CancellationToken);
        return result switch
        {
            CreateUserResult.Created created =>
                new CreateResponse
                {
                    UserId = created.Id.Value
                },
            CreateUserResult.EmailAlreadyRegistered =>
                new CreateResponse
                {
                    EmailAlreadyRegistered =
                        new CreateResponse.Types.EmailAlreadyRegistered()
                },
            CreateUserResult.ParentNotFound =>
                new CreateResponse
                {
                    ParentNotFound = new CreateResponse.Types.ParentNotFound()
                },
            _ => throw new SwitchExpressionException()
        };
    }

    private static User ToDto(UserModel user)
    {
        return new User
        {
            Id = user.Id.Value,
            Name = user.Name,
            FilesCount = user.FilesCount,
            ParentId = user.ParentId?.Value,
            ParentName = user.ParentName
        };
    }

    private static UserWithChildren ToDto(UserWithChildrenModel user)
    {
        return new UserWithChildren
        {
            Id = user.Id.Value,
            Name = user.Name,
            Children = { user.Children.Select(ToDto) }
        };
    }
}
