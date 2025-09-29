namespace memorial_cidade_backend.DTOs;

public class PhotographerDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Biography { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public int PhotosCount { get; set; }   
}