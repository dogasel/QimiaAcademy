using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading;


namespace Business.Abstracts;

public interface IRequestManager
{
    public Task CreateRequestAsync(Request request, CancellationToken cancellationToken);

    public Task<Request> GetRequestByIdAsync(
        long RequestId,
        CancellationToken cancellationToken);

    public Task<List<Request>> GetRequestsAsync(

        CancellationToken cancellationToken);
    public Task UpdateRequestAsync( ///////////////////////////
        long RequestId,
         RequestStatus requestStatus,
        CancellationToken cancellationToken);
    public void DeleteRequestAsync(
       long RequestId,
       CancellationToken cancellationToken);
}
