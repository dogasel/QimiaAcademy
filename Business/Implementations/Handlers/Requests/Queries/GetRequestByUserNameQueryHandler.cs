using AutoMapper;
using Business.Abstracts;
using Business.Implementations.Queries.Requests.Dtos;
using Business.Implementations.Queries.Requests;
using MediatR;

namespace Business.Implementations.Handlers.Requests.Queries;
public class GetRequestByUserNameQueryHandler : IRequestHandler<GetRequestByUserNameQuery, IEnumerable<RequestDto>>
{
    private readonly IRequestManager _requestManager;
    private readonly IUserManager _userManager;
    private readonly IMapper _mapper;

    public GetRequestByUserNameQueryHandler(IRequestManager requestManager, IUserManager userManager, IMapper mapper)
    {
        _requestManager = requestManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RequestDto>> Handle(GetRequestByUserNameQuery request, CancellationToken cancellationToken)
    {
      
        var requestGet = await _requestManager.GetRequestsByUser( request.username, cancellationToken);
        return _mapper.Map<IEnumerable<RequestDto>>(requestGet);
    }
}
