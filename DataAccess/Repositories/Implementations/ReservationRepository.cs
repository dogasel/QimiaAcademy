using DataAccess.Repositories.Abstractions;
using DataAccess.Entities;

namespace DataAccess.Repositories.Implementations;

public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository

{
    public ReservationRepository(QimiaAcademyDbContext dbContext) : base(dbContext)
    {
    }
}