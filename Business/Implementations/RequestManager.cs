using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Exceptions;

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
        request.ID = default;
        await _requestRepository.CreateAsync(request, cancellationToken);
    }


    

    public async Task<IEnumerable<DataAccess.Entities.Request>> GetRequestsAsync( CancellationToken cancellationToken)
    {
        var request = await _requestRepository.GetAllAsync(cancellationToken);
        return request;
    }
    public async Task UpdateRequestAsync( long RequestId, Request updatedrequest, CancellationToken cancellationToken)
    {
        var request=await _requestRepository.GetByIdAsync(RequestId);
        if(updatedrequest.RequestStatus.Equals(RequestStatus.Completed))
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

        request.RequestStatus= updatedrequest.RequestStatus;
        request.UpdateDate=DateTime.Now;
        request.Author = updatedrequest.Author;
        request.Title= updatedrequest.Title;
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

        request.RequestStatus = RequestStatus.Deleted;
        request.UpdateDate= DateTime.Now;
        _requestRepository.UpdateAsync(request, cancellationToken);
    }

    public async Task<DataAccess.Entities.Request> GetRequestByIdAsync(long RequestId, CancellationToken cancellationToken)
    {
        return await _requestRepository.GetByIdAsync(RequestId, cancellationToken);
    }

    public async Task<IEnumerable<Request>> GetRequestsByUser(string username, CancellationToken cancellationToken)
    {
        var result = await _requestRepository.GetByConditionAsync(r => r.UserName == username, cancellationToken);
        if (result == null)
        {
            throw new EntityNotFoundException<Request>("Request not found.");
        }
    
        return result;
    }

}
    

 