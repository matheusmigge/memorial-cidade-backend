using System.ComponentModel.DataAnnotations;

namespace memorial_cidade_backend.Models
{
    public class LocationData
    {
        public int Id { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double Longitude { get; set; }

        [Range(0, 360, ErrorMessage = "Heading must be between 0 and 360 degrees.")]
        public double Heading { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? GoogleEarthPhotoUrl { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? GoogleEarthUrl { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL")]
        public string? GoogleStreetViewEmbedUrl { get; set; }

        public int PhotoId { get; set; }
        public Photo? Photo { get; set; }
    }
}