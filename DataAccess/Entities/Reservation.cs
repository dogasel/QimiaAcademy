using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Reservation
    {
        public long ID { get; set; }

        public long UserId { get; set; }
        public long BookId { get; set; }
        public User? User { get; set; }
        public Book? Book { get; set; }

        public DateTime ReservationDate { get; set; } 

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public DateTime ReservationEndDate { get; set; }

        public bool isDeleted { get; set; }

        [NotMapped]
        public string Title { get; set; }

        [NotMapped]
        public string Author { get; set; }

        [NotMapped]

        public string username { get; set; }
    }
}

