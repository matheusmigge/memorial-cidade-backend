using System.ComponentModel.DataAnnotations;

namespace memorial_cidade_backend.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? IconUrl { get; set; }
        [Required(ErrorMessage = "TagCategory is required")]
        public int TagCategoryId { get; set; }
        public TagCategory TagCategory { get; set; }
    }
}