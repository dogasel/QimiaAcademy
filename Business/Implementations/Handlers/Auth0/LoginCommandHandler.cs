
using Business.Abstracts;
using Business.Implementations.Commands.Books;
using MediatR;

public class LoginCommandHandler : IRequestHandler<LoginCommand, bool>
{
    private readonly IUserManager _userManager;

    public LoginCommandHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return _userManager.TryToLogin(request.auth.mail, request.auth.password, cancellationToken);
    }
}