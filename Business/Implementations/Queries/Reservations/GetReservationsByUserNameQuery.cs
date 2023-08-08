using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Reservations.Dtos;
using MediatR;

namespace Business.Implementations.Queries.Reservations;

public class GetReservationByUserNameQuery : IRequest<IEnumerable<ReservationDto>>
{

    public string username { get; }

    public GetReservationByUserNameQuery(string username)
    {
        this.username = username;
    }
}
