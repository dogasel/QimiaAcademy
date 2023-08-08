using MediatR;
using Business.Abstracts;
using Business.Implementations.Commands.Users;
using DataAccess.Entities;

namespace Business.Implementations.Handlers.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
{
    private readonly IUserManager _userManager;

    public UpdateUserCommandHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstMidName = request.User.FirstMidName,
            LastName = request.User.LastName,
            UpdateDate = DateTime.Now,
            Status = request.User.Status,

        };

        await _userManager.UpdateUserAsync(request.username,user, cancellationToken);

        return user.UserName;
    }
}
