using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Business.Implementations.Queries.Requests.Dtos;


namespace Business.Implementations.Queries.Requests;
public class GetRequestQuery : IRequest<RequestDto>
{
    public long Id { get; }

    public GetRequestQuery(long id)
    {
        Id = id;
    }

}