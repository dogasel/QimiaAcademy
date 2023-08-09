using Business.Abstracts;
using Business.Implementations.Commands.Reservations;
using Business.Implementations.Commands.Users;
using DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Handlers.Reservations.Commands;
public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, long>
{
    private readonly IReservationManager _reservationManager;
  
    private readonly IBookManager _bookManager;

    public UpdateReservationCommandHandler(IReservationManager reservationManager,  IBookManager bookManager)
    {
        _reservationManager = reservationManager;
       
        _bookManager = bookManager;
    }

    public async Task<long> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        

        
        var reservation = new Reservation
        {
            ReservationDate = request.Reservation.ReservationDate,

            UpdateDate = DateTime.Now,
            ReservationEndDate=(request.Reservation.ReservationEndDate)//


        };

        await _reservationManager.UpdateReservationAsync(request.ID,reservation,cancellationToken);

        return reservation.ID;
    }
}