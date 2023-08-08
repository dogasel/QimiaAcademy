using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repositories.Implementations;
using System.Net;
using System.Collections.Generic;

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
        var allBooks = await _bookRepository.GetAllAsync();
        var matchingBooks = allBooks
            .Where(book => book.Title == reservation.Title && book.Author == reservation.Author)
            .ToList();

        var allReservations = await _reservationRepository.GetAllAsync();
        var suitableBook = matchingBooks.FirstOrDefault(book =>
            !allReservations.Any(r =>
                r.BookId == book.ID &&
                !(r.ReservationEndDate < reservation.ReservationDate || r.ReservationDate > reservation.ReservationEndDate)
            )
        );

        if (suitableBook == null)
        {
            throw new InvalidOperationException("No available books found for the desired reservation period.");
        }

        var user = await _userRepository.GetByUserNameAsync(reservation.username, cancellationToken);

        reservation.UserId = user.ID;
        reservation.BookId = suitableBook.ID;
        reservation.ReservationDate = reservation.ReservationDate;
        reservation.ReservationEndDate = reservation.ReservationEndDate;
        reservation.CreationDate = DateTime.Now;
        reservation.UpdateDate = DateTime.Now;
        reservation.isDeleted = false;

        await _reservationRepository.CreateAsync(reservation);
    }

    public async Task<Reservation> GetReservationByIdAsync(long reservationId, CancellationToken cancellationToken)
    {
        // Get Username and Book

        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken); // Assuming you have a GetByIdAsync method in _reservationRepository
        if (reservation == null)
        {
            return null; // Return null if the reservation with the specified ID doesn't exist
        }

        var user = await _userRepository.GetByIdAsync(reservation.UserId, cancellationToken); // Await here
        var book = await _bookRepository.GetByIdAsync(reservation.BookId, cancellationToken);

        var reservationWithRelatedData = new Reservation
        {
            UserId = user.ID,
            BookId = book.ID,
            Title = book.Title,
            Author = book.Author,
            username = user.UserName,
            ReservationDate = reservation.ReservationDate,
            ReservationEndDate = reservation.ReservationEndDate,
            UpdateDate = reservation.UpdateDate,
            CreationDate = reservation.ReservationDate,
        };

        return reservationWithRelatedData;
    }



    async Task<IEnumerable<Reservation>> IReservationManager.GetReservationsAsync(CancellationToken cancellationToken)
    {
        //Get Username and Book

        var reservations = await _reservationRepository.GetAllAsync(cancellationToken);
        var users = await _userRepository.GetAllAsync(cancellationToken); // Await here
        var books = await _bookRepository.GetAllAsync(cancellationToken);

        var reservationsWithRelatedData = reservations
            .Join(
                users, // Use the awaited users variable
                reservation => reservation.UserId,
                user => user.ID,
                (reservation, user) => new { Reservation = reservation, User = user }
            )
            .Join(
                books, // Await the method if needed
                combined => combined.Reservation.BookId,
                book => book.ID,
                (combined, book) => new Reservation
                {
                    UserId = combined.User.ID,
                    BookId = book.ID,
                    Title = book.Title,
                    Author = book.Author,
                    username = combined.User.UserName,
                    ReservationDate = combined.Reservation.ReservationDate,
                    ReservationEndDate = combined.Reservation.ReservationEndDate,
                    UpdateDate = combined.Reservation.UpdateDate,
                    CreationDate = combined.Reservation.ReservationDate,
                }
            );

        return reservationsWithRelatedData;
    }
    public async Task UpdateReservationAsync(long reservationID, Reservation updatedReservation, CancellationToken cancellationToken)
    {
        var oldReservation = await _reservationRepository.GetByIdAsync(reservationID, cancellationToken);

        // Check if the difference between the new end date and reservation date is not more than 14 days
        var maxAllowedEndDate = oldReservation.ReservationEndDate.AddDays(14);
        if (updatedReservation.ReservationEndDate > maxAllowedEndDate)
        {
            throw new InvalidOperationException("End Date must not exceed 14 days from the original reservation date.");
        }

        // Check for conflicting reservations with the same book within the new end date range
        var allReservations = await _reservationRepository.GetAllAsync();
        var conflictingReservations = allReservations
            .Where(r =>
                r.ID != oldReservation.ID &&
                r.BookId == oldReservation.BookId &&
                r.ReservationEndDate >= oldReservation.ReservationEndDate &&
                r.ReservationDate <= updatedReservation.ReservationEndDate
            )
            .ToList();

        if (conflictingReservations.Count > 0)
        {
            throw new InvalidOperationException("Conflicting reservations found for the desired reservation period.");
        }

        // Update the reservation end date and modification date
        oldReservation.ReservationEndDate = updatedReservation.ReservationEndDate;
        oldReservation.UpdateDate = DateTime.Now;

        await _reservationRepository.UpdateAsync(oldReservation, cancellationToken);
    }
    public async Task<IEnumerable<Reservation>> GetReservationsByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        // Get the user with the provided username
        var user = await _userRepository.GetByUserNameAsync(username, cancellationToken);

        if (user == null)
        {
            return Enumerable.Empty<Reservation>(); // Return an empty collection if user doesn't exist
        }

        // Get all reservations associated with the user's ID
        var reservations = await _reservationRepository.GetByConditionAsync(u=>u.UserId==user.ID, cancellationToken);

        // Fetch related book information for each reservation using recursion
        var reservationsWithRelatedData = await FetchReservationDataAsync(reservations, user, cancellationToken);

        return reservationsWithRelatedData;
    }


    
    public async Task<List<Reservation>> FetchReservationDataAsync(IEnumerable<Reservation> reservations, User user, CancellationToken cancellationToken)
    {
        var reservationList = reservations.ToList();

        if (reservationList.Count == 0)
        {
            return new List<Reservation>();
        }

        var reservation = reservationList[0];
        reservationList.RemoveAt(0);

        var book = await _bookRepository.GetByIdAsync(reservation.BookId, cancellationToken);

        var newReservation = new Reservation
        {
            UserId = user.ID,
            BookId = book.ID,
            Title = book.Title,
            Author = book.Author,
            username = user.UserName,
            ReservationDate = reservation.ReservationDate,
            ReservationEndDate = reservation.ReservationEndDate,
            UpdateDate = reservation.UpdateDate,
            CreationDate = reservation.CreationDate,
        };

        var restOfReservations = await FetchReservationDataAsync(reservationList, user, cancellationToken);
        restOfReservations.Insert(0, newReservation);

        return restOfReservations;
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

