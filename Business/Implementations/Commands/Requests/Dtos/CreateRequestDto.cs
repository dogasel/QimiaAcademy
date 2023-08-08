using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Commands.Requests.Dtos
{
    public class CreateRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string username { get; set; }
    
        public RequestStatus RequestStatus { get; set; }

    }
}
