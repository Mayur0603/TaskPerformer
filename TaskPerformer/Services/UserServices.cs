using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskPerformer.Data;
using TaskPerformer.Migrations;
using TaskPerformer.Models;

namespace TaskPerformer.Services
{
    public class UserServices
    {
        private readonly ApplicationDbContext _context;

        public UserServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        {
            if (await _context.users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
                return false;

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password, // You should hash this!
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatDate = DateTime.Now,
                IsActive = true
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> LoginUserAsync(LoginViewModel model)
        {
            return await _context.users
                .FirstOrDefaultAsync(u =>
                    u.Username == model.Username &&
                    u.Password == model.Password && // Should compare hashed
                    u.IsActive);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.users.FindAsync(userId);
        }

        public async Task<UpdateUserResult> UpdateUserAsync(int userId, EditViewModel model)
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null) return new UpdateUserResult { Success = false, ErrorMessage = "User not found" };

            // Check for duplicate username
            bool usernameExists = await _context.users
                .AnyAsync(u => u.Id != userId && u.Username == model.Username);
            if (usernameExists)  
                return new UpdateUserResult { Success = false, ErrorMessage = "This username is already taken." };

            // Check for duplicate email
            bool emailExists = await _context.users
                .AnyAsync(u => u.Id != userId && u.Email == model.Email);
            if (emailExists)
                return new UpdateUserResult { Success = false, ErrorMessage = "This email is already taken." };

            // Update user
            user.Username = model.Username;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            _context.users.Update(user);
            await _context.SaveChangesAsync();

            return new UpdateUserResult { Success = true };
        }

    }
}
