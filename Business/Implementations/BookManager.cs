using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Exceptions;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;

namespace Business.Implementations;


public class BookManager : IBookManager
{
    private readonly IBookRepository _BookRepository;
    private readonly IRequestRepository _requestRepository;
    public BookManager(IBookRepository BookRepository, IRequestRepository RequestRepository)
    {
        _BookRepository = BookRepository;
        _requestRepository = RequestRepository;
    }
    public async Task CreateBookAsync(Book book, CancellationToken cancellationToken)
    {

          await _BookRepository.CreateAsync(book, cancellationToken);
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
    public  async Task<List<Book>> GetBookByAbsoluteDateAsync(DateTime dateTime, CancellationToken cancellationToken)
    {
        var allBooks = await _BookRepository.GetAllAsync(cancellationToken);
        return allBooks
                .Where(book =>
                    (book.Status == BookStatus.OnTheShelf && book.UpdateDate.Date <= dateTime.Date) ||
                    (book.Status != BookStatus.Booked && book.Status != BookStatus.WorkerIsReading && book.UpdateDate.Date == dateTime.Date)
                )
                .ToList();

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

