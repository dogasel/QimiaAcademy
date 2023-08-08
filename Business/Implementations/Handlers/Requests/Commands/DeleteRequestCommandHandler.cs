using Business.Abstracts;
using Business.Implementations.Commands.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Handlers.Requests.Commands;

public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, long>
{
    private readonly IRequestManager _RequestManager;

    public DeleteRequestCommandHandler(IRequestManager RequestManager)
    {
        _RequestManager = RequestManager;
    }

    public async Task<long> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
    {
        _RequestManager.DeleteRequestAsync(request.RequestId, cancellationToken);
         return request.RequestId;

    }
}
