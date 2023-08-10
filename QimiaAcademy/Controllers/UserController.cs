
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Implementations.Commands.Users;
using Business.Implementations.Commands.Users.Dtos;
using Business.Implementations.Queries.Users;
using Business.Implementations.Queries.Users.Dtos;


namespace QimiaAcademy.Controllers
{
    [ApiController]
    [Authorize(Policy = "PermissionPolicy")]
    [Route("[controller]")]

    public class UserController : Controller
    {

       
        private readonly IMediator _mediator;
      
        public UserController(IMediator mediator)
        {
            

            _mediator = mediator;
          
        }


        [HttpPost]
        public async Task<ActionResult> CreateUser(
            [FromBody] CreateUserDto User,
            CancellationToken cancellationToken)
        {
              // Your existing logic to create the user using MediatR
                var response = await _mediator.Send(new CreateUserCommand(User), cancellationToken);
            return CreatedAtAction(
                    nameof(GetUser),
                    new { username = response },
                    User);
        }
    


    [HttpGet("{username}")]
        public Task<UserDto> GetUser(
            [FromRoute] string username,
            CancellationToken cancellationToken)
        {
            return _mediator.Send(
                new GetUserQuery(username),
                cancellationToken);
        }

        [HttpGet]
        public Task<IEnumerable<UserDto>> GetUsers(CancellationToken cancellationToken)
        {
            return _mediator.Send(
                new Business.Implementations.Queries.GetUsersQuery(),
                cancellationToken);
        }

        [HttpPut("{username}")]
        public async Task<ActionResult> UpdateUser(
            [FromRoute] string username,
            [FromBody] CreateUserDto User,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new UpdateUserCommand(User , username),
                cancellationToken);

            return NoContent();
        }

        [HttpDelete("{username}")]
        public async Task<ActionResult> DeleteUser(
            [FromRoute] string username,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeleteUserCommand(username),
                cancellationToken);

            return NoContent();
        }

    }
}
 