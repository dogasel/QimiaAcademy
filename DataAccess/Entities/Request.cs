using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Request
    {
        
       
        public long ID { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;

        public DateTime PublishDate { get; set; }

        public long UserId { get; set; }
        public User? User { get; set; }

        public RequestStatus? RequestStatus;

        public bool isDeleted { get; set; }
        public DateTime UpdateDate { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
