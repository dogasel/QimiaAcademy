using AutoMapper;
using Business.Abstracts;
using Business.Implementations.Queries.Books;
using Business.Implementations.Queries.Books.Dtos;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Handlers.Books.Queries;

public class GetBooksQueryHandler :IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
{
    private readonly IBookManager _bookManager;
    private readonly IMapper _mapper;

    public GetBooksQueryHandler(IBookManager bookManager, IMapper mapper)
    {
        _bookManager = bookManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookManager.GetBooksAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BookDto>>(books)
;
    }
}
