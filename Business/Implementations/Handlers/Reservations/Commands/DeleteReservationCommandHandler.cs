using Business.Abstracts;
using Business.Implementations.Commands.Requests;
using Business.Implementations.Commands.Reservations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Handlers.Reservations.Commands;

public class DeleteReservationCommandHandler  :IRequestHandler<DeleteReservationCommand, long>
{
        private readonly IReservationManager _ReservationManager;

    
    public DeleteReservationCommandHandler(IReservationManager ReservationManager)
    {
        _ReservationManager = ReservationManager;
    }

    public async Task<long> Handle(DeleteReservationCommand reservation, CancellationToken cancellationToken)
    {
        _ReservationManager.DeleteReservationAsync(reservation.ReservationId, cancellationToken);
        return reservation.ReservationId;

    }
}
    

