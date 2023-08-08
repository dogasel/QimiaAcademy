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
    private readonly IUserManager _userManager;
    private readonly IBookManager _bookManager;

    public UpdateReservationCommandHandler(IReservationManager reservationManager, IUserManager userManager, IBookManager bookManager)
    {
        _reservationManager = reservationManager;
        _userManager = userManager;
        _bookManager = bookManager;
    }

    public async Task<long> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserByIdAsync(request.Reservation.username, cancellationToken);

        
        var reservation = new Reservation
        {

            Title=request.Reservation.Title,
            Author=request.Reservation.Author,
            UpdateDate = DateTime.Now,
            ReservationEndDate=request.Reservation.ReservationEndDate,


        };

        await _reservationManager.UpdateReservationAsync(request.ID,reservation,cancellationToken);

        return reservation.ID;
    }
}