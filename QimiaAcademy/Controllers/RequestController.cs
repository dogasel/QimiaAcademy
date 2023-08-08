using MediatR;
using Microsoft.AspNetCore.Mvc;
using Business.Implementations.Commands.Request;
using Business.Implementations.Commands.Requests;
using Business.Implementations.Queries.Requests.Dtos;
using Business.Implementations.Queries.Requests;
using Business.Implementations.Commands.Requests.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace QimiaAcademy.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class RequestController : Controller
{
    private readonly IMediator _mediator;

    public RequestController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<ActionResult> CreateRequest(
        [FromBody] CreateRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateRequestCommand(request), cancellationToken);

        return CreatedAtAction(
            nameof(GetRequest),
            new { id = response },
            request);
    }

    [HttpGet("{id:long}")]
    [Authorize(Policy = "PermissionPolicy")]
    public Task<RequestDto> GetRequest(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        return _mediator.Send(
            new GetRequestQuery(id)
,
            cancellationToken);
    }

    [HttpGet("User/{username}")]
    [Authorize(Policy = "PermissionPolicy")]
    public Task<IEnumerable<RequestDto>> GetRequestsOfUser([FromRoute] string username, CancellationToken cancellationToken)
    {
        return _mediator.Send(
            new GetRequestByUserNameQuery(username),
            cancellationToken);
    }

    [HttpGet ]
    [Authorize(Policy = "PermissionPolicy")]
    public Task<IEnumerable<RequestDto>> GetRequests(CancellationToken cancellationToken)
    {
        return _mediator.Send(
            new GetRequestsQuery(),
            cancellationToken);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "PermissionPolicy")]
    public async Task<ActionResult> UpdateRequest(
        [FromRoute] long id,
        [FromBody] CreateRequestDto request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateRequestCommand(request, id),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]

    public async Task<ActionResult> DeleteRequest(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteRequestCommand(id)
,
            cancellationToken);

        return NoContent();
    }
}