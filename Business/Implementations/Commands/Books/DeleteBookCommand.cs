using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DataAccess.Entities;
namespace Business.Implementations.Commands.Books;

public  class DeleteBookCommand : IRequest<long>
{
    public long BookId { get; set; }
    public DeleteBookCommand(long bookId)
    {
        BookId = bookId;
       
    }
}
