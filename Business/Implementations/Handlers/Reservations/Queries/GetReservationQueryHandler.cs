using AutoMapper;
using Business.Abstracts;
using Business.Implementations.Queries.Reservations.Dtos;
using Business.Implementations.Queries.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Business.Implementations.Handlers.Reservations.Queries;

public class GetReservationQueryHandler : IRequestHandler<GetReservationQuery, ReservationDto>
{
    private readonly IReservationManager _ReservationManager;
    private readonly IMapper _mapper;

    public GetReservationQueryHandler(IReservationManager ReservationManager, IMapper mapper)
    {
        _ReservationManager = ReservationManager;
        _mapper = mapper;
    }

    public async Task<ReservationDto> Handle(GetReservationQuery Reservation, CancellationToken cancellationToken)
    {
        var ReservationGet = await _ReservationManager.GetReservationByIdAsync(Reservation.Id, cancellationToken);
        return _mapper.Map<ReservationDto>(ReservationGet);
    }
}