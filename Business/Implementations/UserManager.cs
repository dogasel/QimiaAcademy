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
        User user= await _UserRepository.GetByUserNameAsync(UserName, cancellationToken);

        if (user.ID != null)
        {
            return user.ID;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="existingUsers"></param>
    /// <returns></returns>
    private string GenerateUserName(string firstName, string lastName, IEnumerable<User> existingUsers)
    {
        string baseUserName = $"{firstName}{lastName}".ToLower().Replace(" ", ""); // Concatenate name and surname, remove spaces, and convert to lowercase

        // Filter the existing users (both active and deleted) with the same baseUserName
        var usersWithSameBaseUserName = existingUsers
            .Where(user => user.UserName.StartsWith(baseUserName))
            .ToList();

        int number = 0;

        // Find the maximum 2-digit number for the existing users with the same name and surname
        if (usersWithSameBaseUserName.Any())
        {
            number = usersWithSameBaseUserName
                .Select(user => user.UserName.Substring(baseUserName.Length))
                .Where(numberPart => long.TryParse(numberPart, out long parsedNumber))
                .Select(parsedNumber => int.Parse(parsedNumber))
                .Max();
        }

        // Increment the number for the new username
        number++;

        // Append the 2-digit number to the baseUserName
        string userName = $"{baseUserName}{number:D2}"; // Format the number with leading zeros

        return userName;
    }



    public async Task<User> GetUserByIdAsync(
        string UserName,
        CancellationToken cancellationToken)
    {
        long id = await GetIdByUserName(UserName, cancellationToken);
        return await _UserRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task UpdateUserAsync(string username, User user, CancellationToken cancellationToken)
    {
        var exuser= await _UserRepository.GetByUserNameAsync(username, cancellationToken);
        exuser.FirstMidName = user.FirstMidName;
        exuser.LastName = user.LastName;
        exuser.Status = user.Status;
        exuser.UpdateDate = user.UpdateDate;
        await _UserRepository.UpdateAsync(exuser, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _UserRepository.GetAllAsync(cancellationToken);
    }

 
    public async Task<string> DeleteAsync(string username,  CancellationToken cancellationToken)
    {
        var User= await _UserRepository.GetByUserNameAsync(username, cancellationToken);
        User.Status = UserStatus.deleted;
        User.UpdateDate = DateTime.Now;
          await _UserRepository.UpdateAsync(User, cancellationToken);
        return username;
    }

}
