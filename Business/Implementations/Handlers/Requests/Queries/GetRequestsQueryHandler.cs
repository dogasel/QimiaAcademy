
using AutoMapper;
using Business.Abstracts;
using Business.Implementations.Queries.Requests;
using Business.Implementations.Queries.Requests.Dtos;
using MediatR;

namespace Business.Implementations.Handlers.Requests.Queries;
public class GetRequestsQueryHandler : IRequestHandler<GetRequestsQuery, IEnumerable<RequestDto>>
{
    private readonly IRequestManager _requestManager;
    private readonly IMapper _mapper;

    public GetRequestsQueryHandler(IRequestManager requestManager, IMapper mapper)
    {
        _requestManager = requestManager;
        _mapper = mapper;
    }

  

    public  async Task<IEnumerable<RequestDto>> Handle(GetRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _requestManager.GetRequestsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RequestDto>>(requests);
    }
}