using Business.Implementations.Queries.Requests.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Queries.Requests;

public class GetRequestsQuery : IRequest<IEnumerable<RequestDto>>
{

}

