using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Commands.Users.Dtos;
using MediatR;
using DataAccess.Entities;
using Azure;

namespace Business.Implementations.Commands.Users;
public class CreateUserCommand : IRequest<string>
{
    public CreateUserDto User { get; set; }

    public CreateUserCommand(
        CreateUserDto user)
    {
       
        this.User = user;
    }
}

