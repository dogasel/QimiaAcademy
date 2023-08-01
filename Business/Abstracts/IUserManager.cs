using DataAccess.Entities;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Threading;


namespace Business.Abstracts;

public interface IUserManager
{
    public Task CreateUserAsync(
        User user,
        CancellationToken cancellationToken);
    public  Task<User> GetUserByIdAsync(
        string UserName,
        CancellationToken cancellationToken);


    public Task<IEnumerable<User>> GetUsersAsync(

        CancellationToken cancellationToken);
    public Task UpdateUserAsync( ///////////////////////////
        long userId,
         User user,
        CancellationToken cancellationToken);
    
    public void DeleteUserAsync(
       long userId,
       CancellationToken cancellationToken);
}
