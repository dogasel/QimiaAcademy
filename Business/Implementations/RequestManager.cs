using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Exceptions;
using Auth0.ManagementApi.Models;


namespace Business.Implementations;
public class RequestManager : IRequestManager
{
    private readonly IBookRepository _bookRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IUserRepository _userRepository;
    private readonly Auth0Token auth0Token;
   public RequestManager(IBookRepository bookRepository, IRequestRepository requestRepository, IUserRepository userRepository, Auth0Token auth0Token)
    {
        _bookRepository = bookRepository;
        _requestRepository = requestRepository;
        _userRepository = userRepository; 
        this.auth0Token = auth0Token;
    }

    
    public async Task CreateRequestAsync(Request request, CancellationToken cancellationToken)
    {
        request.ID = default;
        var existingbooks=await _bookRepository.GetByTAAsync(request.Title,request.Author,cancellationToken);
        if(existingbooks.ToList().Count>0)
        {
            throw new InvalidOperationException("This book is already in the library");
        }
        var isAdmin = await auth0Token.IsAdminAsync();
        if (!isAdmin)
        {
            var username = await auth0Token.GetUsernameFromToken();
            if (!request.UserName.Equals(username))
                throw new InvalidOperationException("Cannot access this request!");
        }
       
        var user = await _userRepository.GetByUserNameAsync(request.UserName, cancellationToken);
        request.userId = user.ID;
        request.UserName = user.UserName;
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
       
        await _requestRepository.UpdateAsync(request, cancellationToken);
           
    }
    public async void DeleteRequestAsync(long RequestId,CancellationToken cancellationToken)
    {
        // Retrieve the reservation from the repository
        var request = _requestRepository.GetByIdAsync(RequestId, cancellationToken).GetAwaiter().GetResult();

        if (request == null)
        {
            throw new InvalidOperationException("request not found.");
        }
        var isAdmin = await auth0Token.IsAdminAsync();
        if(!isAdmin)
        {
            var USERNAME = auth0Token.GetUsernameFromToken();
            if (!request.UserName.Equals(USERNAME))
                throw new InvalidOperationException("Cannot access this request!");
        }
       
        request.RequestStatus = RequestStatus.Deleted;
        request.UpdateDate= DateTime.Now;
        _requestRepository.UpdateAsync(request, cancellationToken);
        
        
    }

    public async Task<DataAccess.Entities.Request> GetRequestByIdAsync(long RequestId, CancellationToken cancellationToken)
    {
        var request= await _requestRepository.GetByIdAsync(RequestId, cancellationToken);
        var isAdmin = await auth0Token.IsAdminAsync();
        if (!isAdmin)
        {
            var USERNAME = auth0Token.GetUsernameFromToken();
            if (!request.UserName.Equals(USERNAME))
                throw new InvalidOperationException("Cannot access this request!");
        }

        return request;
    }

    public async Task<IEnumerable<Request>> GetRequestsByUser(string username, CancellationToken cancellationToken)
    {
        var result = await _requestRepository.GetByConditionAsync(r => r.UserName == username, cancellationToken);
        if (result == null)
        {
            throw new EntityNotFoundException<Request>("Request not found.");
        }
       
        var isAdmin = await auth0Token.IsAdminAsync();
        if (!isAdmin)
        {
            var USERNAME = auth0Token.GetUsernameFromToken();
           
            if (!username.Equals(USERNAME))
                throw new InvalidOperationException("Cannot access this request!");
        }

        return result;
    }

}
    

 