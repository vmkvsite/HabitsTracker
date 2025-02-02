using HabitsTracker.Models;

namespace HabitsTracker.Services
{
    public interface IUserService
    {
        Task<bool> ValidateUserAsync(string username, string password);
        Task<AppUser> CreateUserAsync(string username, string password);
        Task<AppUser?> GetUserByUsernameAsync(string username);
    }
}