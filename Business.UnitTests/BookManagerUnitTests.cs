using Moq;
using Business.Implementations;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;

namespace Business.UnitTests;

[TestFixture]
internal class BookManagerUnitTests
{
    private readonly Mock<IBookRepository> _mockBookRepository;
    private readonly BookManager _BookManager;
    private readonly Mock<IReservationRepository> _mockReservationRepository;
    public BookManagerUnitTests()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _mockReservationRepository = new Mock<IReservationRepository>();
        _BookManager = new BookManager(_mockBookRepository.Object, _mockReservationRepository.Object);


    }

    [Test]
    public async Task CreateBookAsync_WhenCalled_CallsRepository()
    {
        // Arrange
        var testBook = new Book
        {
           Author="Test",
           Title="Test",
           CreationDate= DateTime.Now,
        };

        // Act
        await _BookManager.CreateBookAsync(testBook, default);

        // Assert
        _mockBookRepository
            .Verify(
                sr => sr.CreateAsync(
                    It.Is<Book>(s => s == testBook),
                    It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CreateBookAsync_WhenBookIdHasValue_RemovesAndCallsRepository()
    {
        // Arrange
        var testBook = new Book
        {
            ID = 1,
            Author="Test",
            Title="Test",
            CreationDate= DateTime.Now,
        };

        // Act
        await _BookManager.CreateBookAsync(testBook, default);

        // Assert
        _mockBookRepository
            .Verify(
                sr => sr.CreateAsync(
                    It.Is<Book>(s => s == testBook),
                    It.IsAny<CancellationToken>()), Times.Once);
    }
}
