using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Reservation
    {
        public long ID { get; set; }
        public long UserId { get; set; }
        public long BookId { get; set; }
        public User? User { get; set; }
        public Book? Book { get; set; }

        public BookStatus? BookStatus { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.Now;

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        private DateTime _ReservationEndDate;

        public DateTime ReservationEndDate
        {
            get { return _ReservationEndDate; }
            set
            {
                // Add any custom logic here before setting the value
                // For example, restrict ReservationEndDate not to be earlier than ReservationDate
                if (value >= ReservationDate)
                {
                    _ReservationEndDate = value;
                }
                else
                {
                    // Handle the case when the provided ReservationEndDate is earlier than ReservationDate
                    // For example, you can throw an exception, log an error, or set it to ReservationDate + 7 days
                    _ReservationEndDate = ReservationDate.AddDays(14);
                }
            }
        }

        public bool isDeleted { get; set; }

        public Reservation()
        {
            ReservationEndDate = ReservationDate.AddDays(14);
        }

    
    }
}

