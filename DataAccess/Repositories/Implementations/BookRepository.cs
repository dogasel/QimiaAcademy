using DataAccess.Repositories.Abstractions;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations;

public class BookRepository : RepositoryBase<Book>, IBookRepository

{
    public QimiaAcademyDbContext myContext;


    private readonly DbSet<Book> DbSet;

    public BookRepository(QimiaAcademyDbContext dbContext) : base(dbContext)
    {
        myContext= dbContext;
        DbSet = dbContext.Set<Book>();//dbdeki hazır setleri alıyoz
    }

    public async Task<IEnumerable<Book>> GetByTAAsync(string title, string author, CancellationToken cancellationToken = default)
    {
        // Use EF Core to query the database for the Books with the specified title and author
        var bookList = await DbSet
            .Where(u => u.Title == title && u.Author == author)
            .ToListAsync(cancellationToken);

        // Return the found Books or an empty list if not found
        return bookList;
    }


}