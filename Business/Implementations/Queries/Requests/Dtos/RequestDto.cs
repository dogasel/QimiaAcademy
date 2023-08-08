using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Queries.Requests.Dtos;

public class RequestDto
{
    public long ID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string UserName { get; set; }
    public RequestStatus? RequestStatus { get; set; }

  

}

