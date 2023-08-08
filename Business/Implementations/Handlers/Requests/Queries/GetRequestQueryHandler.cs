using AutoMapper;
using Business.Abstracts;
using Business.Implementations.Queries.Requests;
using Business.Implementations.Queries.Requests.Dtos;
using MediatR;

namespace Business.Implementations.Handlers.Requests.Queries;
public class GetRequestQueryHandler : IRequestHandler<GetRequestQuery, RequestDto>
{
    private readonly IRequestManager _requestManager;
    private readonly IMapper _mapper;

    public GetRequestQueryHandler(IRequestManager requestManager, IMapper mapper)
    {
        _requestManager = requestManager;
        _mapper = mapper;
    }

    public async Task<RequestDto> Handle(GetRequestQuery request, CancellationToken cancellationToken)
    {
        var requestGet = await _requestManager.GetRequestByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<RequestDto>(requestGet);
    }
}