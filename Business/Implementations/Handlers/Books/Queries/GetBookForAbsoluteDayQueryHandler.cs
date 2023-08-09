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

public class GetBookForAbsoluteDayQueryHandler : IRequestHandler<GetBookForAbsoluteDayQuery, IEnumerable<BookDto>>
{
    private readonly IBookManager _bookManager;
    private readonly IMapper _mapper;

    public GetBookForAbsoluteDayQueryHandler(IBookManager bookManager, IMapper mapper)
    {
        _bookManager = bookManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetBookForAbsoluteDayQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookManager.GetBookByAbsoluteDateAsync(request.newdate, cancellationToken);
        return _mapper.Map<IEnumerable<BookDto>>(book);
    }
}

    
