using MediatR;
using System.Threading;
using Business.Abstracts;
using Business.Implementations.Commands.Users;

namespace Business.Implementations.Handlers.Users.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, string>
    {
        private readonly IUserManager _userManager;

        public DeleteUserCommandHandler(IUserManager userManager)
        {
            _userManager = userManager;

        }

        public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userManager.DeleteAsync(request.UserName, cancellationToken);
            return request.UserName;
        }

    }
}
