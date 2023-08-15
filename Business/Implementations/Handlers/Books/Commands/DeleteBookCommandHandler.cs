using Business.Abstracts;
using Business.Implementations.Commands.Books;
using DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Implementations.Handlers.Books.Commands;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, long>
{
    private readonly IBookManager _BookManager;

    public DeleteBookCommandHandler(IBookManager bookManager)
    {
        _BookManager = bookManager;
    }

    public async Task<long> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        await _BookManager.DeleteBookAsync(request.BookId, cancellationToken);
        return request.BookId;

    }
}