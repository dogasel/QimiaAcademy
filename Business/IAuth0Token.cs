using System.Threading.Tasks;

namespace Business
{
    public interface IAuth0Token
    {
        Task<string> GetAccessToken(string email, string password);
        Task<string> GetUsernameFromToken();
        Task<bool> IsAdminAsync();
    }
}
