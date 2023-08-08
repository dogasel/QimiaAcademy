
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Implementations.Commands.Books;
using Business.Implementations.Commands.Books.Dtos;
using Business.Implementations.Queries.Books;
using Business.Implementations.Queries.Books.Dtos;


namespace QimiaAcademy.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class BookController : Controller
    {
        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<ActionResult> CreateBook(
            [FromBody] CreateBookDto Book,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new CreateBookCommand(Book), cancellationToken);

            return CreatedAtAction(
                nameof(GetBook),
                new { id = response },
                Book);
        }

        [HttpGet("{id}")]
        public Task<BookDto> GetBook(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            return _mediator.Send(
                new GetBookQuery(id),
                cancellationToken);
        }

        [HttpGet]
        public Task<IEnumerable<BookDto>> GetBooks(CancellationToken cancellationToken)
        {
            return _mediator.Send(
                new Business.Implementations.Queries.Books.GetBooksQuery(),
                cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(
            [FromRoute] long id,
            [FromBody] CreateBookDto Book,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new UpdateBookCommand(Book, id),
                cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeleteBookCommand(id),
                cancellationToken);

            return NoContent();
        }

    }
}
