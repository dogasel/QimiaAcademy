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

    public Task<long> GetIdByUserName(
        string UserName, CancellationToken cancellationToken
        );
    public Task<IEnumerable<User>> GetUsersAsync(

        CancellationToken cancellationToken);
    public Task UpdateUserAsync( ///////////////////////////
        string username,
         User user,
        CancellationToken cancellationToken);
    
    public Task<string> DeleteAsync(
       string username, 
       CancellationToken cancellationToken);
    public Task<bool> TryToLogin(
        string email,
        string password, 
        CancellationToken cancellationToken);
}
