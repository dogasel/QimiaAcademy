using MediatR;
using Microsoft.AspNetCore.Mvc;
using Business.Implementations.Commands.Reservations;
using Business.Implementations.Commands.Reservations;
using Business.Implementations.Queries.Reservations.Dtos;
using Business.Implementations.Queries.Reservations;
using Business.Implementations.Commands.Reservations.Dtos;

namespace QimiaAcademy.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : Controller
{
    private readonly IMediator _mediator;

    public ReservationController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<ActionResult> CreateReservation(
        [FromBody] CreateReservationDto Reservation,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateReservationCommand(Reservation), cancellationToken);

        return CreatedAtAction(
            nameof(GetReservation),
            new { id = response },
            Reservation);
    }

    [HttpGet("{id:long}")]
    public Task<ReservationDto> GetReservation(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        return _mediator.Send(
            new GetReservationQuery(id)
,
            cancellationToken);
    }

    [HttpGet("User/{username}")]
    public Task<IEnumerable<ReservationDto>> GetReservationsOfUser([FromRoute] string username, CancellationToken cancellationToken)
    {
        return _mediator.Send(
            new GetReservationByUserNameQuery(username),
            cancellationToken);
    }

    [HttpGet]
    public Task<IEnumerable<ReservationDto>> GetReservations(CancellationToken cancellationToken)
    {
        return _mediator.Send(
            new GetReservationsQuery(),
            cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReservation(
        [FromRoute] long id,
        [FromBody] CreateReservationDto Reservation,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateReservationCommand(Reservation, id),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteReservationCommand(id)
,
            cancellationToken);

        return NoContent();
    }
}