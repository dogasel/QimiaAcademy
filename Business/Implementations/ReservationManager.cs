using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repositories.Implementations;
using System.Net;

namespace Business.Implementations;

public class ReservationManager : IReservationManager
{
    private readonly IBookRepository _bookRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    public ReservationManager(IBookRepository bookRepository, IReservationRepository reservationRepository, IUserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
    }

    public async Task CreateReservationAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(reservation.BookId, cancellationToken);

        // Check if the reservation date is the current day
        if (reservation.ReservationDate.Date == DateTime.Now.Date)
        {
            // Check if the book is on the shelf on the current day
            if (book.Status != BookStatus.OnTheShelf)
            {
                // The book is not on the shelf on the current day, don't create the reservation.
                // You can handle this scenario as you see fit, for example, throw an exception or return a specific error code.
                throw new InvalidOperationException("The book is not available on the shelf on the current day.");
            }
        }
        else
        {
            // Check if the book is in the "WorkerIsReading" status or "Booked" status on the desired reservation date
            if (book.Status == BookStatus.WorkerIsReading || book.Status == BookStatus.Booked)
            {
                // The book is either in "WorkerIsReading" or "Booked" status on the desired reservation date.
                // Don't create the reservation.
                // You can handle this scenario as you see fit, for example, throw an exception or return a specific error code.
                throw new InvalidOperationException("The book is not available for reservation on the desired date.");
            }

            // Check if there are other reservations for any copy of the same book on the desired reservation date
            var otherReservations = await _reservationRepository.GetByConditionAsync(r =>
                r.Book.Title == book.Title && r.ReservationDate.Date == reservation.ReservationDate.Date,
                cancellationToken);

            // Check if there are any available copies of the book on the desired reservation date
            var availableCopies = otherReservations.Any(r => r.BookStatus == null || r.BookStatus == BookStatus.OnTheShelf);

            // If there are no available copies, don't create the reservation
            if (!availableCopies)
            {
                // There are no available copies of the book on the desired reservation date, don't create the reservation.
                // You can handle this scenario as you see fit, for example, throw an exception or return a specific error code.
                throw new InvalidOperationException("There are no available copies of the book on the desired reservation date.");
            }
        }

        // Proceed with creating the reservation.
        reservation.CreationDate = DateTime.Now;
        reservation.UpdateDate = DateTime.Now;
        reservation.isDeleted = false;

        // Set the reservation's end date based on the reservation date
        reservation.ReservationEndDate = reservation.ReservationDate.AddDays(14);

        // Save the reservation to the database using the _reservationRepository
        await _reservationRepository.CreateAsync(reservation, cancellationToken);

    }
    public async Task<Reservation> GetReservationByIdAsync(long ReservationId, CancellationToken cancellationToken)
    {
        var res = await _reservationRepository.GetByIdAsync(ReservationId, cancellationToken);
        return res;
    }

    public async Task<List<Reservation>> GetReservationsAsync(CancellationToken cancellationToken)
    {
        // Retrieve all not deleted reservations from the repository
        var reservations = await _reservationRepository.GetByConditionAsync(r => !r.isDeleted, cancellationToken);

        // Convert the result to a list and return
        return reservations.ToList();
    }
    public async Task UpdateReservationAsync(long reservationId, Reservation updatedReservation, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
        if (reservation == null)
        {
            throw new InvalidOperationException("Reservation not found.");
        }

        var book = await _bookRepository.GetByIdAsync(reservation.BookId, cancellationToken);
        if (book == null)
        {
            throw new InvalidOperationException("Book not found.");
        }

        if (reservation.ReservationDate.Date == DateTime.Now.Date)
        {
            // If it's the current day reservation, only cancel reservation is allowed
            if (updatedReservation.isDeleted)
            {
                // If canceled, change the book status to "OnTheShelf"
                book.Status = BookStatus.OnTheShelf;
            }
            else
            {
                throw new InvalidOperationException("Updating reservation is not allowed on the current day.");
            }
        }
        else
        {
            // Check if the book is in the "WorkerIsReading" status on the reservation date
            if (book.Status == BookStatus.WorkerIsReading && book.UpdateDate.Date == reservation.ReservationDate.Date)
            {
                // If the book is in "WorkerIsReading" status on the reservation date, the book can be delivered.
                // We don't need to set isDeleted to true here since we're not canceling the reservation.
                book.Status = BookStatus.OnTheShelf;
            }
            else
            {
                // If it's a future reservation date, we can extend the delivery date if there are no other reservations
                var otherReservations = await _reservationRepository.GetByConditionAsync(r =>
                    r.BookId == reservation.BookId && r.ReservationDate.Date == reservation.ReservationDate.Date,
                    cancellationToken);

                // Check if there are other reservations for the same book on the same reservation date
                if (!otherReservations.Any(r => r.ID != reservationId))
                {
                    // There are no other reservations, so we can extend the delivery date.
                    reservation.ReservationEndDate = reservation.ReservationEndDate.AddDays(7);
                }
                else
                {
                    // There are other reservations for the same book on the same reservation date, so we can't extend the delivery date.
                    throw new InvalidOperationException("There are other reservations for the same book on the same reservation date.");
                }
            }
        }

        // Update the reservation details and book status
        reservation.UpdateDate = DateTime.Now;
        reservation.isDeleted = updatedReservation.isDeleted;

        // Save the updated reservation to the database using the _reservationRepository
        await _reservationRepository.UpdateAsync(reservation, cancellationToken);

        // Save the updated book status to the database using the _bookRepository
        await _bookRepository.UpdateAsync(book, cancellationToken);
    }
    public async Task<List<Reservation>> GetReservationsByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var userId = await _userRepository.GetByUserNameAsync(username, cancellationToken);
        

        if (userId == -1)
        {
            throw new InvalidOperationException("User not found.");
        }
        

        // Retrieve all reservations associated with the user
        var userReservations = await _reservationRepository.GetByConditionAsync(r => r.UserId == userId, cancellationToken);

        return userReservations.ToList();
    }

    public void DeleteReservationAsync(long reservationId, CancellationToken cancellationToken)
    {
        // Retrieve the reservation from the repository
        var reservation = _reservationRepository.GetByIdAsync(reservationId, cancellationToken).GetAwaiter().GetResult();

        if (reservation == null)
        {
            throw new InvalidOperationException("Reservation not found.");
        }

        // Get the tarih işlem yapıldığı tarihi temsil eden değişken
        var reservationDate = reservation.ReservationDate.Date;

        // Retrieve all reservations associated with the user
        var userReservations = _reservationRepository.GetByConditionAsync(r => r.UserId == reservation.UserId, cancellationToken).GetAwaiter().GetResult();

        // Find the reservation to delete using the provided reservationId and reservation date
        var reservationToDelete = userReservations.FirstOrDefault(r => r.ID == reservationId && r.ReservationDate.Date >= reservationDate);

        if (reservationToDelete != null)
        {
            // Cancel the postdate reservation by marking it as deleted
            reservationToDelete.isDeleted = true;

            // Save the updated reservation to the database using the _reservationRepository
            _reservationRepository.UpdateAsync(reservationToDelete, cancellationToken).GetAwaiter().GetResult();
        }
    }
}

