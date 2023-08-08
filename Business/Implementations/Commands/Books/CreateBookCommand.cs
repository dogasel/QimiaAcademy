using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Implementations.Commands.Books.Dtos;
using MediatR;
using DataAccess.Entities;

namespace Business.Implementations.Commands.Books;

public class CreateBookCommand : IRequest<long>
{
    public CreateBookDto Book { get; set; }

    public CreateBookCommand(
        CreateBookDto book)
    {
        this.Book = book;
    }
}
