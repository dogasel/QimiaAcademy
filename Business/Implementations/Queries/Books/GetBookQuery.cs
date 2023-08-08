using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Queries.Books.Dtos;
using MediatR;

namespace Business.Implementations.Queries.Books;

public class GetBookQuery : IRequest<BookDto>
{
    public long Id { get; }

    public GetBookQuery(long id)
    {
        Id = id;
    }

}


