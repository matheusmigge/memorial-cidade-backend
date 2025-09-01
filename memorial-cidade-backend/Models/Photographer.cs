using System.ComponentModel.DataAnnotations;

namespace memorial_cidade_backend.Models
{
    public class Photographer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(150, ErrorMessage = "Name can have a maximum of 150 characters")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Biography can have a maximum of 500 characters")]
        public string? Biography { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
