using Business.Implementations.Queries.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Queries.Books;

public class GetBooksQuery : IRequest<IEnumerable<BookDto>>
{

}