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

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, long>
{
    private readonly IBookManager _bookManager;

    public UpdateBookCommandHandler(IBookManager bookManager)
    {
        _bookManager = bookManager;
    }
    public async Task<long> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Title = request.Book.Title,
            Author = request.Book.Author,
            CreationDate = DateTime.Now,
            Status = request.Book.Status,

        };


        await _bookManager.UpdateBookAsync(request.bookId, book, cancellationToken);

        return book.ID;
    }
}
