using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Users.Dtos;
using Business.Implementations.Queries.Books.Dtos;

namespace Business.Implementations.Queries.Reservations.Dtos;

public class ReservationDto
{
    public long ID { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime ReservationEndDate { get; set; }

    public string username { get; set; }

}
