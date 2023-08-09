using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Books.Dtos;
using MediatR;

namespace Business.Implementations.Queries.Books;

public class GetBookForAbsoluteDayQuery : IRequest<IEnumerable<BookDto>>
{
    public DateTime newdate { get; }

    public GetBookForAbsoluteDayQuery(DateTime newdate)
    {
        this.newdate = newdate;
    }

}


