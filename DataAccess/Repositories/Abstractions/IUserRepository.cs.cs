using DataAccess.Repositories.Abstractions;
using DataAccess.Entities;

namespace DataAccess.Repositories.Abstractions;

public interface IUserRepository : IRepositoryBase<User>
{
    //Task<T> GetByUserNameAsync(string username, CancellationToken cancellationToken = default);
    Task <long>GetByUserNameAsync(string userName,CancellationToken cancellationToken=default);
}
