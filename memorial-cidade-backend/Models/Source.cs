using memorial_cidade_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace memorial_cidade_backend.Models
{
    public class Source
    {
        public int Id { get; set; }
        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? DocumentPhotoUrl { get; set; }
        public string? Collection { get; set; }
        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? SourceUrl { get; set; }
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}