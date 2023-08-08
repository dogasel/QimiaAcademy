using Azure.Core;
using Business.Implementations.Commands.Books.Dtos;
using Business.Implementations.Commands.Requests.Dtos;
using DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Commands.Books;

public class UpdateBookCommand : IRequest<long>
{
    public long bookId { get; set; }
    public CreateBookDto Book { get; set; }

    public UpdateBookCommand(CreateBookDto book, long bookId)
    { 
        this.Book = book;
        this.bookId = bookId;
       
    }
}
