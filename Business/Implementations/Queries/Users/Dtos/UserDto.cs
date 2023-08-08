using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Reservations.Dtos;

namespace Business.Implementations.Queries.Users.Dtos;

public class UserDto
{
    public int ID { get; set; }
    public string? LastName { get; set; }
    public string? FirstMidName { get; set; }
    public string? UserName { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime CreateDate { get; set; }
    public UserStatus Status { get; set; }

    public ICollection<ReservationDto> Reservations { get; set; }
 
}

