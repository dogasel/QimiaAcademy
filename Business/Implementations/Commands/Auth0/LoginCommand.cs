using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Commands.Books.Dtos;
using MediatR;
using DataAccess.Entities;
using Business.Implementations.Commands.Auth0;

namespace Business.Implementations.Commands.Books;

public class LoginCommand : IRequest<bool>
{
    public AuthDto auth { get; set; }

    public LoginCommand(
        AuthDto auth)
    {
        this.auth = auth;
    }
}
