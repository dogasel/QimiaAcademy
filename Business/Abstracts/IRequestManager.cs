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

    public Task<IEnumerable<Request>> GetRequestsAsync(

        CancellationToken cancellationToken);
    public Task UpdateRequestAsync( 
        long RequestId,
         Request request,
        CancellationToken cancellationToken);
    public void DeleteRequestAsync(
       long RequestId,
       CancellationToken cancellationToken);

    public Task<IEnumerable<Request>> GetRequestsByUser(
        string username,
        CancellationToken cancellationToken);
}
