using Business.Abstracts;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;

namespace Business.Implementations;

public class UserManager : IUserManager
{
    private readonly IUserRepository _UserRepository;
    public UserManager(IUserRepository UserRepository)
    {
        _UserRepository = UserRepository;
    }

    public async Task <long> GetIdByUserName(
        string UserName, CancellationToken cancellationToken
        )
    {
        long UserID = await _UserRepository.GetByUserNameAsync(UserName, cancellationToken);

        if (UserID != null)
        {
            return UserID;
        }

        return -1;
    }
    public async Task CreateUserAsync(
        User user,
        CancellationToken cancellationToken)
    {
        // No id should be provided while insert.
        user.ID = default;
        user.UserName= GenerateUserName(user.FirstMidName, user.LastName, await _UserRepository.GetAllAsync(cancellationToken));

        await _UserRepository.CreateAsync(user, cancellationToken);
    }
    private string GenerateUserName(string firstName, string lastName, IEnumerable<User> existingUsers)
    {
        string baseUserName = $"{firstName}{lastName}".ToLower().Replace(" ", ""); // Concatenate name and surname, remove spaces, and convert to lowercase
        string userName = baseUserName;
        long number = 0;

        // Find the maximum 2-digit number for the existing users with the same name and surname
        foreach (var user in existingUsers)
        {
            if (user.UserName.StartsWith(baseUserName))
            {
                string numberPart = user.UserName.Substring(baseUserName.Length);
                if (long.TryParse(numberPart, out long existingNumber) && existingNumber >= number)
                {
                    number = existingNumber + 1;
                }
            }
        }

        // Append the 2-digit number to the baseUserName
        if (number > 0 && number <= 99)
        {
            userName += number.ToString("D2"); // Format the number with leading zeros
        }

        return userName;
    }

    public async Task<User> GetUserByIdAsync(
        string UserName,
        CancellationToken cancellationToken)
    {
        long id = await GetIdByUserName(UserName, cancellationToken);
        return await _UserRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task UpdateUserAsync(long userId, User user, CancellationToken cancellationToken)
    {
        await _UserRepository.UpdateAsync(user , cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _UserRepository.GetAllAsync(cancellationToken);
    }

    public async void DeleteUserAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _UserRepository.GetByIdAsync(userId, cancellationToken);
        user.Status = UserStatus.deleted;

        await  _UserRepository.UpdateAsync(user, cancellationToken);
    }
}
