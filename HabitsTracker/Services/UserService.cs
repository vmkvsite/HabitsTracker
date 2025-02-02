using HabitsTracker.Data;
using HabitsTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HabitsTracker.Services
{
    public class UserService : IUserService
    {
        private readonly HabitsDbContext _context;

        public UserService(HabitsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return false;

            var hashedPassword = HashPassword(password);
            return user.PasswordHash == hashedPassword;
        }

        public async Task<AppUser> CreateUserAsync(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = new AppUser
            {
                Username = username,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Habits)
                .ThenInclude(h => h.Completions)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}