using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Business.Implementations.Commands.Requests.Dtos;


namespace Business.Implementations.Commands.Request;

public class CreateRequestCommand : IRequest<long>
{
    public CreateRequestDto Request { get; set; }

    public CreateRequestCommand(CreateRequestDto request)
    {

        Request = request;
    }
}