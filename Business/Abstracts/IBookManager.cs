using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading;


namespace Business.Abstracts;

public interface IBookManager
{
    public Task CreateBookAsync(
        Book book,
        CancellationToken cancellationToken);

    public Task<Book> GetBookByIdAsync(
        long bookId,
        CancellationToken cancellationToken);

    public Task<List<Book>> GetBooksAsync(

        CancellationToken cancellationToken);
    public Task<List<Book>> GetBookByAbsoluteDateAsync(/////////

        DateTime dateTime,CancellationToken cancellationToken);

    public Task UpdateBookAsync( 
        long bookId,
        Book book,
        CancellationToken cancellationToken);
    public void DeleteBookAsync(
       long bookId,
       CancellationToken cancellationToken);
}
