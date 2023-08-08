
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Business.Implementations.Commands.Users.Dtos;

namespace Business.Implementations.Commands.Users;
public class UpdateUserCommand : IRequest<string>
{
    public CreateUserDto User { get; set; }
    public string username { get; set; }
    public UpdateUserCommand(CreateUserDto user,string username)
    {
      
        this.User = user;
        this.username = username;
    }
}

