using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Repositories.Abstractions;
using DataAccess.Exceptions;

namespace DataAccess.Repositories.Implementations
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly DbSet<User> DbSet;
        public QimiaAcademyDbContext mycontext;
        
        public UserRepository(QimiaAcademyDbContext dbContext ) : base(dbContext)
        {
            mycontext = dbContext;
            DbSet = dbContext.Set<User>();
        }

        public async Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {

            // Use EF Core to query the database for the user with the specified UserName
            User user = await DbSet
                .SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken);

            // Return the found user or null if not found
            return user;
        }


        public async Task<bool> TryToLogin(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await DbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException<User>("User with this email not found!");
            }

            if (VerifyPassword(password, user.Password))
            {
                return true;
            }

            throw new Exception("Password incorrect. Please try again!");
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            if (enteredPassword.Equals(storedPasswordHash))
            {
                return true;
            }
            return false;
        }
    }
}
