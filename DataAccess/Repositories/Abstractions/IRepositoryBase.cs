using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstractions;

public interface IRepositoryBase<T> where T : class
{

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    //Task GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    void DeleteAsync(long id, CancellationToken cancellationToken = default);
}
