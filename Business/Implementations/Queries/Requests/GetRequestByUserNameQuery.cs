using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Requests.Dtos;
using MediatR;

namespace Business.Implementations.Queries.Requests;

public class GetRequestByUserNameQuery :IRequest <IEnumerable<RequestDto>>
{

    public string username { get; }

    public GetRequestByUserNameQuery(string username)
    {
        this.username = username;
    }
}
