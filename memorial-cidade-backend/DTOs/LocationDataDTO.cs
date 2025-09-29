namespace memorial_cidade_backend.DTOs;

public class LocationDataDTO
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Heading { get; set; }
    public string? GoogleEarthPhotoUrl { get; set; }
    public string? GoogleEarthUrl { get; set; }
    public string? GoogleStreetViewEmbedUrl { get; set; }
}