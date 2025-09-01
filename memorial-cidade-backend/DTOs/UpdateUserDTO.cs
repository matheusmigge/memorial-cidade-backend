namespace memorial_cidade_backend.DTOs
{
    public class UpdateUserDTO
    {
        public string? Email { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Bio { get; set; }
    }
}
