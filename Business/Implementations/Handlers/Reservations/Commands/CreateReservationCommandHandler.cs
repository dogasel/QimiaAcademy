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
public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, long>
{
    private readonly IReservationManager _reservationManager;
    private readonly IUserManager _userManager;
    private readonly IBookManager _bookManager;

    public CreateReservationCommandHandler(IReservationManager reservationManager)
    {
        _reservationManager = reservationManager;
       
    }

    public async Task<long> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
       
        
        var reservation = new Reservation
        {
           
            username = request.Reservation.username,
            Title= request.Reservation.Title,
            Author=request.Reservation.Author,
            CreationDate = DateTime.Now,
            ReservationEndDate = request.Reservation.ReservationDate.AddDays(14),
            ReservationDate=request.Reservation.ReservationDate,

        };

        await _reservationManager.CreateReservationAsync(reservation, cancellationToken);

        return reservation.ID;
    }
}