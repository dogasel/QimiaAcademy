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
using DataAccess.Exceptions;
using System.Diagnostics.Eventing.Reader;
using Auth0.ManagementApi.Models;
using User = DataAccess.Entities.User;


namespace Business.Implementations;

public class ReservationManager : IReservationManager
{
    private readonly IBookRepository _bookRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly Auth0Token auth0Token;
   public ReservationManager(IBookRepository bookRepository, IReservationRepository reservationRepository, IUserRepository userRepository, Auth0Token auth0Token)
    {
        _bookRepository = bookRepository;
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        this.auth0Token = auth0Token;
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
        var isAdmin = await auth0Token.IsAdminAsync();
        if (!isAdmin)
        {
            var USERNAME = auth0Token.GetUsernameFromToken();

            if (!reservation.username.Equals(USERNAME))
                throw new InvalidOperationException("Cannot create this reservation because of the user mismatch!");
        }
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
       
        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken); 
        if (reservation == null)
        {
            return null; // Return null if the reservation with the specified ID doesn't exist
        }
        var isAdmin = await auth0Token.IsAdminAsync();
        if (!isAdmin)
        {
            var USERNAME = await auth0Token.GetUsernameFromToken();
            var userToken = await _userRepository.GetByUserNameAsync(USERNAME);
            if (reservation.UserId != userToken.ID)
                throw new InvalidOperationException("Cannot access this reservation due to the user mismatch!");
        }

        var user = await _userRepository.GetByIdAsync(reservation.UserId, cancellationToken);
        var book = await _bookRepository.GetByIdAsync(reservation.BookId, cancellationToken);

        var reservationWithRelatedData = new Reservation
        {
            ID= reservation.ID,
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

        return reservationWithRelatedData;
    }



    async Task<IEnumerable<Reservation>> IReservationManager.GetReservationsAsync(CancellationToken cancellationToken)
    {
     

        var reservations = await _reservationRepository.GetAllAsync(cancellationToken);
        var users = await _userRepository.GetAllAsync(cancellationToken); 
        var books = await _bookRepository.GetAllAsync(cancellationToken);

        var reservationsWithRelatedData = reservations
            .Join(
                users, 
                reservation => reservation.UserId,
                user => user.ID,
                (reservation, user) => new { Reservation = reservation, User = user }
            )
            .Join(
                books,
                combined => combined.Reservation.BookId,
                book => book.ID,
                (combined, book) => new Reservation
                {
                    ID=combined.Reservation.ID,//idlerin tutması için
                    UserId = combined.User.ID,
                    BookId = book.ID,
                    Title = book.Title,
                    Author = book.Author,
                    username = combined.User.UserName,
                    ReservationDate = combined.Reservation.ReservationDate,
                    ReservationEndDate = combined.Reservation.ReservationEndDate,
                    UpdateDate = combined.Reservation.UpdateDate,
                    CreationDate = combined.Reservation.CreationDate,
                }
            );

        return reservationsWithRelatedData;
    }
    public async Task UpdateReservationAsync(long reservationID, Reservation updatedReservation, CancellationToken cancellationToken)
    {
        
       
        var oldReservation = await _reservationRepository.GetByIdAsync(reservationID, cancellationToken);
        var isAdmin = await auth0Token.IsAdminAsync();
        if (!isAdmin)
        {
            var USERNAME = await auth0Token.GetUsernameFromToken();
            var userToken = await _userRepository.GetByUserNameAsync(USERNAME);
            if (oldReservation.UserId != userToken.ID)
                throw new InvalidOperationException("Cannot access this reservation due to the user mismatch!");
        }
        if (updatedReservation.ReservationDate.Date < DateTime.Now.Date)
        {
            throw new InvalidOperationException("It is not post date reservation. ");
        }
        if (updatedReservation.ReservationDate> DateTime.Now)
        {
            var allReservations = await _reservationRepository.GetAllAsync();
           
            var conflictingReservations = allReservations
                .Where(r =>
                    r.ID != oldReservation.ID &&
                    r.BookId == oldReservation.BookId &&
                   !(r.ReservationEndDate < updatedReservation.ReservationDate || r.ReservationDate > updatedReservation.ReservationDate.AddDays(14))
                )
                .ToList();

            if (conflictingReservations.Count > 0)
            {
                throw new InvalidOperationException("This book is not valid for this day!");
            }
            oldReservation.ReservationDate = updatedReservation.ReservationDate;

            oldReservation.ReservationEndDate = updatedReservation.ReservationDate.AddDays(14);
          
        }

        else
        {

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

            

            // Update the reservation end date and update date
            oldReservation.ReservationEndDate = updatedReservation.ReservationEndDate;
            
        }
     

        oldReservation.UpdateDate = DateTime.Now;

        await _reservationRepository.UpdateAsync(oldReservation, cancellationToken);


    }
    public async Task<IEnumerable<Reservation>> GetReservationsByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var isAdmin = await auth0Token.IsAdminAsync();
        if (!isAdmin)
        {
            var USERNAME = auth0Token.GetUsernameFromToken();

            if (!username.Equals(USERNAME))
                throw new InvalidOperationException("Cannot get this reservation because of the user mismatch!");
        }
 
        var user = await _userRepository.GetByUserNameAsync(username, cancellationToken);

        if (user == null)
        {
            return Enumerable.Empty<Reservation>();
        }

       
        var reservations = await _reservationRepository.GetByConditionAsync(u=>u.UserId==user.ID, cancellationToken);

      
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






    public async Task DeleteReservationAsync(long reservationId, CancellationToken cancellationToken)
    {
        // Retrieve the reservation from the repository
        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);

        if (reservation == null)
        {
            throw new InvalidOperationException("Reservation not found.");
        }

        var isAdmin = await auth0Token.IsAdminAsync();

        if (!isAdmin)
        {
            var USERNAME = auth0Token.GetUsernameFromToken();

            if (!reservation.username.Equals(USERNAME))
                throw new InvalidOperationException("Cannot delete this reservation because of the user mismatch!");
        }

        var reservationDate = reservation.ReservationDate;
        var userReservations = await _reservationRepository.GetByConditionAsync(r => r.UserId == reservation.UserId, cancellationToken);

        var reservationToDelete = userReservations.FirstOrDefault(r => r.ID == reservationId && r.ReservationDate >= reservationDate);

        if (reservationToDelete != null)
        {
            reservationToDelete.isDeleted = true;
            await _reservationRepository.UpdateAsync(reservationToDelete, cancellationToken);
        }
    }

}

