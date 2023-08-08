using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Business.Implementations.Commands.Users;
public class DeleteUserCommand : IRequest<string>
{
    public string UserName { get; set; }

    public DeleteUserCommand(string username )
    {
        this.UserName = username;
       
    }
}
