using System.ComponentModel.DataAnnotations;

namespace memorial_cidade_backend.Models
{
    public class TagCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can have a maximum of 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can have a maximum of 500 characters")]
        public string Description { get; set; }
    }
}