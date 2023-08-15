using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Exceptions;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;
using Microsoft.AspNetCore.Identity;

namespace Business.Implementations;


public class BookManager : IBookManager
{
    private readonly IBookRepository _BookRepository;
    private readonly IReservationRepository _ReservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuth0Token auth0token;
    private readonly IRequestRepository requestRepository;
    public BookManager(IBookRepository BookRepository, IReservationRepository ReservationRepository, IUserRepository userRepository, IAuth0Token auth0token, IRequestRepository requestRepository)
    {
        _BookRepository = BookRepository;
        _ReservationRepository = ReservationRepository;
        _userRepository = userRepository;
        this.auth0token = auth0token;
        this.requestRepository = requestRepository;
    }
    
    public async Task CreateBookAsync(Book book, CancellationToken cancellationToken)
    {

        var bookList = await _BookRepository.GetByConditionAsync(
                        b => b.Title == book.Title && b.Author == book.Author,
                        cancellationToken);
        if (bookList.ToList().Count > 0)
        {
            book.RequestId = bookList.First().RequestId;
            await _BookRepository.CreateAsync(book, cancellationToken);
        }
        else
        {


            var username = await auth0token.GetUsernameFromToken();
            var user = await _userRepository.GetByUserNameAsync(username, cancellationToken);


            var request = new Request
            {
                // Set properties of the request as needed
                Title = book.Title,
                Author = book.Author,
                UserName = username,
                userId = user.ID,
                CreateDate= DateTime.Now,
            };

            await requestRepository.CreateAsync(request, cancellationToken);

            // Now retrieve the newly created request to get its ID
            var newRequest = await requestRepository.GetByConditionAsync(
                r => r.Title == request.Title && r.Author == request.Author && r.UserName == request.UserName,
                cancellationToken
            );

            if (newRequest != null)
            {
                var updatedRequest = newRequest.First();
                updatedRequest.RequestStatus = RequestStatus.Completed;
           
                await requestRepository.UpdateAsync(updatedRequest, cancellationToken);
                var boook = new Book
                {
                    Title = updatedRequest.Title,
                    Author = updatedRequest.Author,
                    RequestId = updatedRequest.ID,
                    CreationDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                await _BookRepository.CreateAsync(boook, cancellationToken);

            }
            else
            {
                throw new InvalidOperationException("Failed to retrieve newly created request.");
            }

        }

    }         
        
    
    public async Task<IEnumerable<Book>> GetBookByTAAsync(string title, string author, CancellationToken cancellationToken)
    {
        var book = await _BookRepository.GetByTAAsync(title,author ,cancellationToken);
        return book;

    }
    public async Task<Book> GetBookByIDAsync(long ID, CancellationToken cancellationToken)
    {
        return await _BookRepository.GetByIdAsync(ID, cancellationToken);
    }

    public async Task<List<Book>> GetBooksAsync(CancellationToken cancellationToken)
    {
        var allBooks = await _BookRepository.GetAllAsync(cancellationToken);
        return allBooks.Where(book => book.Status != BookStatus.Deleted).ToList();
    }
    public async Task<IEnumerable<Book>> GetBookByAbsoluteDateAsync(DateTime dateTime, CancellationToken cancellationToken)
    {
        // Get all reservations on the specified date
        var reservationsOnDate = await _ReservationRepository.GetByConditionAsync(r =>
            r.ReservationDate.Date <= dateTime.Date && r.ReservationEndDate.Date >= dateTime.Date,
            cancellationToken);

        // Get all books that are not included in reservations on the specified date
        var bookedBookIds = reservationsOnDate.Select(r => r.BookId).ToList();

        var availableBooks = await _BookRepository.GetByConditionAsync(b =>
            !bookedBookIds.Contains(b.ID),
            cancellationToken);

        return availableBooks;
    }



public async Task UpdateBookAsync(long bookId, Book book, CancellationToken cancellationToken)
    {
        var OLDBook=await _BookRepository.GetByIdAsync(bookId);
        OLDBook.Title=book.Title;
        OLDBook.Author = book.Author;
        OLDBook.Status = book.Status;
        OLDBook.UpdateDate = book.UpdateDate;
        await _BookRepository.UpdateAsync(OLDBook, cancellationToken);

    }
    public async Task <long> DeleteBookAsync(long bookId, CancellationToken cancellationToken)
    {
        var book = await _BookRepository.GetByIdAsync(bookId, cancellationToken);
        book.Status = BookStatus.Deleted;

        await _BookRepository.UpdateAsync(book, cancellationToken);
        return bookId;
    }
}

