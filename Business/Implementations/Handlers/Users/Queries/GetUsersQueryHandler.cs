using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Users.Dtos;
using Business.Implementations.Queries;
using Business.Abstracts;

namespace Business.Implementations.Handlers.Users.Queries;


public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
private readonly IUserManager _userManager;
private readonly IMapper _mapper;

public GetUsersQueryHandler(
    IUserManager userManager,
    IMapper mapper)
{
    _userManager = userManager;
    _mapper = mapper;
}

public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
{
    var users = await _userManager.GetUsersAsync(cancellationToken);

    return _mapper.Map<IEnumerable<UserDto>>(users);
}
}