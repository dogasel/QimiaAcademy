using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Exceptions;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;

namespace Business.Implementations;


public class BookManager : IBookManager
{
    private readonly IBookRepository _BookRepository;
    private readonly IReservationRepository _ReservationRepository;
    public BookManager(IBookRepository BookRepository, IReservationRepository ReservationRepository)
    {
        _BookRepository = BookRepository;
        _ReservationRepository = ReservationRepository;
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
            throw new EntityNotFoundException<Book>("This book does not exist in the library.Books that do not exist in the library will be handle on request. Please create a request for this book.");
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

