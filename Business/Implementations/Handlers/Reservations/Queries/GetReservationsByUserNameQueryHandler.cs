using AutoMapper;
using Business.Abstracts;
using Business.Implementations.Queries.Reservations.Dtos;
using Business.Implementations.Queries.Reservations;
using MediatR;

namespace Business.Implementations.Handlers.Reservations.Queries;
public class GetReservationByUserNameQueryHandler : IRequestHandler<GetReservationByUserNameQuery, IEnumerable<ReservationDto>>
{
    private readonly IReservationManager _ReservationManager;
    private readonly IUserManager _userManager;
    private readonly IMapper _mapper;

    public GetReservationByUserNameQueryHandler(IReservationManager ReservationManager, IUserManager userManager, IMapper mapper)
    {
        _ReservationManager = ReservationManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReservationDto>> Handle(GetReservationByUserNameQuery Reservation, CancellationToken cancellationToken)
    {

        var ReservationGet = await _ReservationManager.GetReservationsByUsernameAsync(Reservation.username, cancellationToken);
        return _mapper.Map<IEnumerable<ReservationDto>>(ReservationGet);
    }
}
