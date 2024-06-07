using C2S.Business.Models;
using C2S.Data.Enumrations;
using C2S.Data.Models;

namespace C2S.Business.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterationRequest registerationModel, Role role); // Method for user registration
        Task<bool> LoginAsync(LoginModel loginModel); // Method for user login
        Task<bool> UserExistsAsync(string email); // Check if a user with the given email already exists
        Task<User> GetUserByEmail(string email); // Generate authentication token for a user
    }
}
