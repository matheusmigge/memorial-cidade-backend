namespace memorial_cidade_backend.DTOs;

public class CreatePhotoDTO
{
    public string Url { get; set; }
    public string Title { get; set; }
    public int YearStart { get; set; }
    public int? YearEnd { get; set; }
    public string? UserNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? PhotographerId { get; set; }
    public LocationDataDTO? LocationData { get; set; }
    public int SourceId { get; set; }
    public int UserId { get; set; }
}