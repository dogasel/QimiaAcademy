using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Business.Implementations.Commands.Reservations;
public class DeleteReservationCommand : IRequest<long>
{
    public long ReservationId { get; set; }
    public DeleteReservationCommand(long reservationId)
    {
        this.ReservationId = reservationId;
    }
}
