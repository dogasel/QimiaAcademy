using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Commands.Reservations.Dtos;
using MediatR;
using DataAccess.Entities;
using Azure.Core;

namespace Business.Implementations.Commands.Reservations;

public class CreateReservationCommand :IRequest<long>
{
    public CreateReservationDto Reservation { get; set; }

    public CreateReservationCommand(CreateReservationDto reservation)
    {
 
        this.Reservation = reservation;
    }
}

    

