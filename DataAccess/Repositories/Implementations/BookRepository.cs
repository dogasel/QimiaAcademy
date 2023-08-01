using DataAccess.Repositories.Abstractions;
using DataAccess.Entities;


namespace DataAccess.Repositories.Implementations;

public class BookRepository : RepositoryBase<Book>, IBookRepository

{
    public BookRepository(QimiaAcademyDbContext dbContext) : base(dbContext)
    {
    }
}