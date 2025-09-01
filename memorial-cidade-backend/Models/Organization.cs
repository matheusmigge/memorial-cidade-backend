using System.ComponentModel.DataAnnotations;

namespace memorial_cidade_backend.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(150, ErrorMessage = "Name can have a maximum of 150 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description can have a maximum of 500 characters")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? LogoUrl { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? WebsiteUrl { get; set; }
    }
}
