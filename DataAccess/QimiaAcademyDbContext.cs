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

        modelBuilder.Entity<User>()
       .HasIndex(u => u.UserName)
       .IsUnique();
        modelBuilder.Entity<User>()
     .HasQueryFilter(x => !x.Status.Equals (UserStatus.deleted));
        modelBuilder.Entity<Book>()
            .HasQueryFilter(x => !x.Status.Equals(BookStatus.Deleted));
        modelBuilder.Entity<Reservation>()
            .HasQueryFilter(x => x.isDeleted == false);
        modelBuilder.Entity<Request>()
             .HasQueryFilter(x => !x.RequestStatus.Equals(RequestStatus.Deleted));


        base.OnModelCreating(modelBuilder);


    }
}
