using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Implementations.Commands.Users;
using Business.Implementations.Commands.Users.Dtos;
using Business.Implementations.Queries.Users;
using Business.Implementations.Queries.Users.Dtos;
using MediatR;
using Business.Implementations.Commands.Books;
using Business.Implementations.Commands.Auth0;
using Business;
using System.Text;

namespace QimiaAcademy.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AuthController : Controller
    {

        private readonly StringBuilder messageBuilder;
        private readonly IMediator _mediator;
        private readonly Auth0Token accessToken;
        public AuthController(IMediator mediator, Auth0Token accessToken)
        {
            messageBuilder=new StringBuilder();
            _mediator = mediator;
            this.accessToken = accessToken;
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(
            [FromBody] AuthDto auth,
            CancellationToken cancellationToken)
        {
            // Your existing logic to create the user using MediatR
            var response = await _mediator.Send(new LoginCommand(auth), cancellationToken);
            if (response)
            {
                var token= await accessToken.GetAccessToken(auth.mail, auth.password);
                
                messageBuilder.AppendLine("Login successful.");
                messageBuilder.AppendLine($"Your token: {token}");
                return Ok(messageBuilder.ToString());

            }
            return Unauthorized(new { message = "Invalid email or password" });
        }
    }
}   