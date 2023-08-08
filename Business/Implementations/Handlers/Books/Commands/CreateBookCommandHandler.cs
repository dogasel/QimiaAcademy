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

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, long>
{
    private readonly IBookManager _bookManager;

    public CreateBookCommandHandler(IBookManager bookManager)
    {
        _bookManager = bookManager;
    }
    public async Task<long> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Title = request.Book.Title,
            Author = request.Book.Author,
            CreationDate = DateTime.Now,
            Status=BookStatus.OnTheShelf,

        };

        await _bookManager.CreateBookAsync(book, cancellationToken);

        return book.ID;
    }
}
