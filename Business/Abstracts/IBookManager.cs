using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading;


namespace Business.Abstracts;

public interface IBookManager
{
    public Task CreateBookAsync(
        Book book,
        CancellationToken cancellationToken);

    public Task<IEnumerable<Book>> GetBookByTAAsync(
        string author, string title,
        CancellationToken cancellationToken);
    public Task<Book> GetBookByIDAsync(long ID, CancellationToken cancellationToken);
    

    public Task<List<Book>> GetBooksAsync(

        CancellationToken cancellationToken);
    public Task<List<Book>> GetBookByAbsoluteDateAsync(/////////

        DateTime dateTime,CancellationToken cancellationToken);

    public Task UpdateBookAsync( 
        long bookId,
        Book book,
        CancellationToken cancellationToken);
    public Task<long> DeleteBookAsync(
       long bookId,
       CancellationToken cancellationToken);
}
