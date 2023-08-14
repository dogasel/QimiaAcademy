
using DataAccess.Entities;

namespace DataAccess.Repositories.Abstractions;

public interface IUserRepository : IRepositoryBase<User>
{
  
    Task <User>GetByUserNameAsync(string userName,CancellationToken cancellationToken=default);
    public Task<bool> TryToLogin(string email, string password, CancellationToken cancellationToken);

}
