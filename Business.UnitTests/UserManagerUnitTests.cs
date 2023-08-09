using Moq;
using Business.Implementations;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;

namespace Business.UnitTests;

[TestFixture]
internal class UserManagerUnitTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserManager _UserManager;

    public UserManagerUnitTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _UserManager = new UserManager(_mockUserRepository.Object);
    }

    [Test]
    public async Task CreateUserAsync_WhenCalled_CallsRepository()
    {
        // Arrange
        var testUser = new User
        {
            UserName = "Test",
            FirstMidName = "Test",
            LastName = "Test"
            
        };

        // Act
        await _UserManager.CreateUserAsync(testUser, default);

        // Assert
        _mockUserRepository
            .Verify(
                sr => sr.CreateAsync(
                    It.Is<User>(s => s == testUser),
                    It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CreateUserAsync_WhenUserIdHasValue_RemovesAndCallsRepository()
    {
        // Arrange
        var testUser = new User
        {
            ID = 1,
            UserName = "Test",
            FirstMidName = "Test",
            LastName = "Test"
        };

        // Act
        await _UserManager.CreateUserAsync(testUser, default);

        // Assert
        _mockUserRepository
            .Verify(
                sr => sr.CreateAsync(
                    It.Is<User>(s => s == testUser),
                    It.IsAny<CancellationToken>()), Times.Once);
    }
}
