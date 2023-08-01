using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using DataAccess.Exceptions;
using System.Linq.Expressions;
using System.Security.Principal;
using DataAccess;
using DataAccess.Repositories.Abstractions;
using System.Threading;


namespace DataAccess.Repositories.Implementations;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected QimiaAcademyDbContext DbContext { get; set; }

    private readonly DbSet<T> DbSet;

    protected RepositoryBase(QimiaAcademyDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(expression).ToListAsync(cancellationToken);
    }
    
    public async Task<T> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {

        return await DbSet.FindAsync(
            id,
            cancellationToken) ??
            throw new EntityNotFoundException<T>(id);
    }

    public async Task DeleteByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        DbContext.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateAsync(
        T entity,
        CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(
            entity,
            cancellationToken);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async void DeleteAsync(
        long id,
        CancellationToken cancellationToken)
    {
        
        T Result=  await GetByIdAsync(id, cancellationToken);
        DbSet.Remove(Result);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

}
