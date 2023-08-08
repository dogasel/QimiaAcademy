using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Commands.Reservations.Dtos
{
    public class CreateReservationDto
    {
        public string username { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ReservationEndDate { get; set; }
        public string Author { get; set; }
        public  string Title { get; set; }

    }
}
