using Business.Implementations.Commands.Reservations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Commands.Reservations;

public class UpdateReservationCommand : IRequest<long>

{
    public long ID { get; set; }
    public CreateReservationDto Reservation { get; set; }

    public UpdateReservationCommand(CreateReservationDto reservation, long ID)
    {
        this.Reservation = reservation;
        this.ID = ID;
    }
}


