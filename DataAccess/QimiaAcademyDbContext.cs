using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DataAccess;

public class QimiaAcademyDbContext : DbContext
{
    public QimiaAcademyDbContext(
        DbContextOptions<QimiaAcademyDbContext> contextOptions) : base(contextOptions)
    {
    }

    public DbSet<User> Users{ get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Request> Requests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<User>()
               .Property(u => u.UserName)
              
        .HasComputedColumnSql("[FirstMidName] + [LastName] + RIGHT('00' + CAST(ISNULL((SELECT MAX(CAST(SUBSTRING([UserName], LEN([UserName]) - 1, 2) AS INT)) + 1) , 0) AS NVARCHAR(2)), 2)")

       
        .IsRequired();*/
        base.OnModelCreating(modelBuilder);
    }
}
