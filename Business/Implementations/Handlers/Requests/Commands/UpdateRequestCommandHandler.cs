
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

public class UpdateRequestCommandHandler : IRequestHandler<UpdateRequestCommand, long>
{
    private readonly IRequestManager _requestManager;

    public UpdateRequestCommandHandler(IRequestManager requestManager)
    {
        _requestManager = requestManager;

    }

    public async Task<long> Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
    {

        var requestt = new DataAccess.Entities.Request
        {
           
            RequestStatus = request.Request.RequestStatus,
            UpdateDate = DateTime.Now,
            
        };

        await _requestManager.UpdateRequestAsync(request.id,requestt, cancellationToken);///////////////
       
        return requestt.ID;
    }
}