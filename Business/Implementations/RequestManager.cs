using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Azure.Core;
using Request = DataAccess.Entities.Request;

namespace Business.Implementations;
public class RequestManager : IRequestManager
{
    private readonly IBookRepository _bookRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IUserRepository _userRepository;
    public RequestManager(IBookRepository bookRepository, IRequestRepository requestRepository, IUserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _requestRepository = requestRepository;
        _userRepository = userRepository; 
    }

    
    public async Task CreateRequestAsync(Request request, CancellationToken cancellationToken)
    {
        
        // Set the request's creation date to the current date/time
        request.CreationDate = DateTime.Now;

        // Set the request's status to Pending
        request.RequestStatus = RequestStatus.Pending;

        // Set the update date to the current date/time
        request.UpdateDate = DateTime.Now;

        await _requestRepository.CreateAsync(request, cancellationToken);
    }


    

    public async Task<List<Request>> GetRequestsAsync( CancellationToken cancellationToken)
    {
        var request = await _requestRepository.GetAllAsync(cancellationToken);
        return request.ToList();
    }
    public async Task UpdateRequestAsync( long RequestId, RequestStatus requestStatus, CancellationToken cancellationToken)
    {
        var request=await _requestRepository.GetByIdAsync(RequestId);
        if(requestStatus.Equals(RequestStatus.Completed))
        {
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                RequestId = request.ID,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
            await _bookRepository.CreateAsync(book,cancellationToken);

        }

        request.RequestStatus=requestStatus;
        request.UpdateDate=DateTime.Now;
        await _requestRepository.UpdateAsync(request, cancellationToken);
           
    }
    public void DeleteRequestAsync(long RequestId,CancellationToken cancellationToken)
    {
        // Retrieve the reservation from the repository
        var request = _requestRepository.GetByIdAsync(RequestId, cancellationToken).GetAwaiter().GetResult();

        if (request == null)
        {
            throw new InvalidOperationException("request not found.");
        }

        request.isDeleted = true;
        _requestRepository.UpdateAsync(request, cancellationToken);
    }

    public async Task<DataAccess.Entities.Request> GetRequestByIdAsync(long RequestId, CancellationToken cancellationToken)
    {
        return await _requestRepository.GetByIdAsync(RequestId, cancellationToken);
    }

  
}
    

 