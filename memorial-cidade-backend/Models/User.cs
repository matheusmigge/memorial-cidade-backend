using System.ComponentModel.DataAnnotations;
using memorial_cidade_backend.DTOs;

namespace memorial_cidade_backend.Models
{
    public enum UserRole
    {
        User,
        Admin
    }
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password hash is required")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Password salt is required")]
        public string PasswordSalt { get; set; }
        [StringLength(100, ErrorMessage = "First name can have a maximum of 100 characters")]
        public string? FirstName { get; set; }
        [StringLength(100, ErrorMessage = "Last name can have a maximum of 100 characters")]
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        [StringLength(500, ErrorMessage = "Biography can have a maximum of 500 characters")]
        public string? Bio { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}