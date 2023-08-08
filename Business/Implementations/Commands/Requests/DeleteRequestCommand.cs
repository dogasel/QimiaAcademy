
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DataAccess.Entities;
namespace Business.Implementations.Commands.Requests;

public class DeleteRequestCommand :IRequest<long>
{
    public long  RequestId { get; set; }
    public DeleteRequestCommand(long requestId)
    {
        this.RequestId = requestId;
    }
}
