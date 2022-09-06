using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Airlines25554.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;

        public UserHelper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // -> Adicionar User
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        // -> Devolve o user a partir do ID
    
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
