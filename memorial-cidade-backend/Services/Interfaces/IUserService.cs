using memorial_cidade_backend.Models;
using memorial_cidade_backend.DTOs;

namespace memorial_cidade_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> CreateAsync(CreateUserDTO userDto);
        Task<UserDTO> UpdateAsync(int id, UpdateUserDTO userDto);
        Task DeleteAsync(int id);
        Task<UserDTO> LoginAsync(LoginDTO loginDto);
        Task<UserDTO> ChangeRoleAsync(int id, UserRole newRole);
        Task<UserDTO> ToggleActiveStatusAsync(int id);
        Task<bool> ValidatePasswordAsync(int userId, string password);
        Task<IEnumerable<UserDTO>> SearchByEmailAsync(string email);
    }
}