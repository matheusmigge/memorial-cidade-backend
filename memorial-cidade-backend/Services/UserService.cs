using memorial_cidade_backend.Data;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.DTOs;
using memorial_cidade_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace memorial_cidade_backend.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        private static UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Bio = user.Bio,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive
            };
        }

        private static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
            }
        }

        private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
                return computedHash == storedHash;
            }
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            return MapToDTO(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _context.Users
                .OrderBy(u => u.Email)
                .ToListAsync();

            return users.Select(MapToDTO);
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already registered.");

            CreatePasswordHash(userDto.Password, out string passwordHash, out string passwordSalt);

            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                BirthDate = userDto.BirthDate,
                Bio = userDto.Bio,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToDTO(user);
        }

        public async Task<UserDTO> UpdateAsync(int id, UpdateUserDTO userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            if (!string.IsNullOrEmpty(userDto.Email) && userDto.Email != user.Email)
            {
                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                    throw new InvalidOperationException("Email already registered.");
                user.Email = userDto.Email;
            }

            if (!string.IsNullOrEmpty(userDto.CurrentPassword) && !string.IsNullOrEmpty(userDto.NewPassword))
            {
                if (!VerifyPasswordHash(userDto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                    throw new InvalidOperationException("Current password is incorrect.");

                CreatePasswordHash(userDto.NewPassword, out string passwordHash, out string passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            user.FirstName = userDto.FirstName ?? user.FirstName;
            user.LastName = userDto.LastName ?? user.LastName;
            user.BirthDate = userDto.BirthDate ?? user.BirthDate;
            user.Bio = userDto.Bio ?? user.Bio;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDTO(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDTO> LoginAsync(LoginDTO loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                throw new KeyNotFoundException("Invalid email or password.");

            if (!user.IsActive)
                throw new InvalidOperationException("Account is deactivated.");

            if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                throw new InvalidOperationException("Invalid email or password.");

            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapToDTO(user);
        }

        public async Task<UserDTO> ChangeRoleAsync(int id, UserRole newRole)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            user.Role = newRole;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDTO(user);
        }

        public async Task<UserDTO> ToggleActiveStatusAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            user.IsActive = !user.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDTO(user);
        }

        public async Task<bool> ValidatePasswordAsync(int userId, string password)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
        }

        public async Task<IEnumerable<UserDTO>> SearchByEmailAsync(string email)
        {
            var users = await _context.Users
                .Where(u => u.Email.Contains(email))
                .OrderBy(u => u.Email)
                .ToListAsync();

            return users.Select(MapToDTO);
        }
    }
}