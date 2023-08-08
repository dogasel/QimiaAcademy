using Business.Implementations.Commands.Requests.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Business.Implementations.Commands.Requests;
public class UpdateRequestCommand : IRequest<long>
{
    public CreateRequestDto Request { get; set; }
    public long id { get; set; }
    public UpdateRequestCommand(CreateRequestDto request,long id)
    {
        this.id= id;
        this.Request = request;
    }
}
