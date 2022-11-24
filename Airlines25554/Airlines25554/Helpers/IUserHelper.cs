using Airlines25554.Data.Entities;
using Airlines25554.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Airlines25554.Helpers
{
    public interface IUserHelper
    {
        // Método que permite obter um user através do seu email
        Task<User>  GetUserByUserNameAsync(string userName);

        // Método que vai servir para criar um utilizador.
        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

         Task<IdentityResult> UpdateUserAsync(User user);

         Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
       

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string userId);

        Task<User> GetUserByEmailAsync(string email);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}
