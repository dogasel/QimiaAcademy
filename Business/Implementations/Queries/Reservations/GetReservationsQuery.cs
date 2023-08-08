using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Business.Implementations.Queries.Reservations.Dtos;

namespace Business.Implementations.Queries.Reservations;

public class GetReservationsQuery :IRequest<IEnumerable<ReservationDto>>
{

}

