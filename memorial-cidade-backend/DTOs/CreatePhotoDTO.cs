namespace memorial_cidade_backend.DTOs;

public class CreatePhotoDTO
{
    public string Url { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int YearStart { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? PhotographerId { get; set; }
    public int SourceId { get; set; }
    public int UserId { get; set; }
    public LocationDataDTO? LocationData { get; set; }
}