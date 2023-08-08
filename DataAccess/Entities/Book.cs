using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Book
    {
        public long ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;

        public long RequestId { get; set; }
       
        public Request? Request { get; set; }
        public DateTime UpdateDate { get; set; }
        public BookStatus Status { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
