using AutoMapper;
using Business.Abstracts;
using Business.Implementations.Queries.Books;
using Business.Implementations.Queries.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Handlers.Books.Queries;

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
{
    private readonly IBookManager _bookManager;
    private readonly IMapper _mapper;

    public GetBookQueryHandler(IBookManager bookManager, IMapper mapper)
    {
        _bookManager = bookManager;
        _mapper = mapper;
    }

    public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookManager.GetBookByIDAsync(request.Id, cancellationToken);
        return _mapper.Map<BookDto>(book);
    }
}
