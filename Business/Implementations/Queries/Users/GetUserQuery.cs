using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Business.Implementations.Queries.Users.Dtos;


namespace Business.Implementations.Queries.Users;
public class GetUserQuery :IRequest<UserDto>
{
    public string username { get; }

    public GetUserQuery(string username)
    {
        this.username = username;
    }

}

