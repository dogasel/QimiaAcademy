using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Business.Implementations.Queries.Reservations.Dtos;


namespace Business.Implementations.Queries.Reservations;
public class GetReservationQuery : IRequest<ReservationDto>
{
    public long Id { get; }

    public GetReservationQuery(long id)
    {
        Id = id;
    }

}

