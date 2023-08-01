using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Repositories.Abstractions;

namespace DataAccess.Repositories.Implementations
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly DbSet<User> DbSet;
        
        public UserRepository(QimiaAcademyDbContext dbContext,DbSet<User> _DbSet) : base(dbContext)
        {
            DbSet = _DbSet;
        }

        public async Task<long> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {

            // Use EF Core to query the database for the user with the specified UserName
            User user = await DbSet
                .SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken);

            // Return the found user or null if not found
            return user.ID;
        }
    }
}
