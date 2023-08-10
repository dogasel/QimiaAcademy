using MediatR;
using Business.Abstracts;
using Business.Implementations.Commands.Users;
using DataAccess.Entities;

namespace Business.Implementations.Handlers.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,string>
{
    private readonly IUserManager _userManager;

    public CreateUserCommandHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstMidName = request.User.FirstMidName,
            LastName = request.User.LastName,
            CreationDate = DateTime.Now,
            Password = request.User.Password,
            
        };

        await _userManager.CreateUserAsync(user, cancellationToken);

        return user.UserName;
    }
}
