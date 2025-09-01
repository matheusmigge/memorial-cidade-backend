using Azure;
using memorial_cidade_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace memorial_cidade_backend.Models
{
    public class Photo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Photo URL is required")]
        [Url(ErrorMessage = "Please provide a valid URL")]
        public string Url { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title can have a maximum of 200 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The year the photo was taken is required")]
        public int YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string? UserNote { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public int? PhotographerId { get; set; }
        public Photographer? Photographer { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public int LocationId { get; set; }
        public LocationData Location { get; set; }

        [Required(ErrorMessage = "Source is required")]
        public int SourceId { get; set; }
        public Source Source { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
