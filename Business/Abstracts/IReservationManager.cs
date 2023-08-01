using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading;


namespace Business.Abstracts;

public interface IReservationManager
{
    public Task CreateReservationAsync(
        Reservation Reservation,
        CancellationToken cancellationToken);

    public Task<Reservation> GetReservationByIdAsync(
        long ReservationId,
        CancellationToken cancellationToken);

    public Task<List<Reservation>> GetReservationsAsync(

        CancellationToken cancellationToken);
    public Task UpdateReservationAsync( ///////////////////////////emin değilim
        long ReservationId,
        Reservation reservation,
        CancellationToken cancellationToken);
    public void DeleteReservationAsync(
       long ReservationId,
       CancellationToken cancellationToken);
}
