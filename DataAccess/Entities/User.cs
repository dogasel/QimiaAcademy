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
   
    public long ID { get; set; }
    public string UserName { get;  set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FirstMidName { get; set; } = string.Empty;
    public UserStatus Status { get; set; } = UserStatus.working;

    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }

    public ICollection<Reservation>? Reservations { get; set; }
}
