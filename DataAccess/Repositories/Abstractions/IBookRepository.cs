using DataAccess.Repositories.Abstractions;
using DataAccess.Entities;
using DataAccess.Repositories.Implementations;

namespace DataAccess.Repositories.Abstractions;

public interface IBookRepository : IRepositoryBase<Book>
{
    public Task<IEnumerable<Book>> GetByTAAsync(string title, string author, CancellationToken cancellationToken = default);
    
}