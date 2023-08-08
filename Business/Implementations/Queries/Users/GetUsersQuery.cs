using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Users.Dtos;

namespace Business.Implementations.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
}

