using Business.Abstracts;
using Business.Implementations.Commands.Request;
using Business.Implementations.Commands.Requests;
using Business.Implementations.Commands.Reservations;
using DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Handlers.Requests.Commands;

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, long>
{
    private readonly IRequestManager _requestManager;
    private readonly IUserManager _userManager;
    
    public CreateRequestCommandHandler(IRequestManager requestManager, IUserManager userManager)
    {
        _requestManager = requestManager;
        _userManager = userManager;
        
    }

    public async Task<long> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserByIdAsync(request.Request.username, cancellationToken);
        var requestt = new Request
        {
           Title= request.Request.Title,
           Author=request.Request.Author,
           RequestStatus= RequestStatus.Pending,
           UserName = user.UserName,
           userId= user.ID,

        };

        await _requestManager.CreateRequestAsync(requestt, cancellationToken);

        return requestt.ID;
    }
}
