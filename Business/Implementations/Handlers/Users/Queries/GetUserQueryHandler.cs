using AutoMapper;
using MediatR;
using Business.Abstracts;
using Business.Implementations.Queries.Users;
using Business.Implementations.Queries.Users.Dtos;
using Business.Implementations.Queries;
using DataAccess.Entities;

namespace Business.Implementations.Handlers.Users.Queries;
public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserManager _userManager;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(
        IUserManager userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserByIdAsync(request.username, cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}

    
