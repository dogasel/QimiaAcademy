using DataAccess.Repositories.Abstractions;
using DataAccess.Entities;


namespace DataAccess.Repositories.Implementations;

public class RequestRepository : RepositoryBase<Request>, IRequestRepository

{
    public RequestRepository(QimiaAcademyDbContext dbContext) : base(dbContext)
    {
    }
}