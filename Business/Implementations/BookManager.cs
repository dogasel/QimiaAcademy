using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;

namespace Business.Implementations;


public class BookManager : IBookManager
{
    private readonly IBookRepository _BookRepository;
    public BookManager(IBookRepository BookRepository)
    {
        _BookRepository = BookRepository;
    }
    public async Task CreateBookAsync(Book book, CancellationToken cancellationToken)
    {
        book.ID = default;
        await _BookRepository.CreateAsync(book, cancellationToken);
    }

    public async Task<Book> GetBookByIdAsync(long bookId, CancellationToken cancellationToken)
    {
        var book = await _BookRepository.GetByIdAsync(bookId, cancellationToken);
        return book;
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
        await _BookRepository.UpdateAsync(book, cancellationToken);

    }
    public async void DeleteBookAsync(long bookId, CancellationToken cancellationToken)
    {
        var book = await _BookRepository.GetByIdAsync(bookId, cancellationToken);
        book.Status = BookStatus.Deleted;

        await _BookRepository.UpdateAsync(book, cancellationToken);
    }
}

