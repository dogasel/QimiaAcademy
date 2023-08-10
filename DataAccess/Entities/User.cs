using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities;

public class User
{
    public string Email
    { get; set; } = string.Empty!;
    public string Password { get; set; } = string.Empty!;
    public long ID { get; set; }
    public string UserName { get;  set; } = string.Empty!;
    public string LastName { get; set; } = string.Empty!;
    public string FirstMidName { get; set; } = string.Empty!;
    public UserStatus Status { get; set; } 

    public DateTime CreationDate { get; set; } 
    public DateTime? UpdateDate { get; set; }

    public ICollection<Reservation>? Reservations { get; set; }
}
