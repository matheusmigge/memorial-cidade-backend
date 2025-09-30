using memorial_cidade_backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;

namespace memorial_cidade_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotographerController : ControllerBase
    {
        private readonly IPhotographerService _photographerService;

        public PhotographerController(IPhotographerService photographerService)
        {
            _photographerService = photographerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photographer>>> GetAll()
        {
            var photographers = await _photographerService.GetAllAsync();
            var photographerDtos = photographers.Select(p => new PhotographerDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Biography = p.Biography,
                BirthDate = p.BirthDate,
                DeathDate = p.DeathDate,
                PhotosCount = p.Photos?.Count ?? 0
            });
            return Ok(photographerDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Photographer>> GetById(int id)
        {
            try
            {
                var photographer = await _photographerService.GetByIdAsync(id);
                var photographerDto = new PhotographerDTO
                {
                    Id = photographer.Id,
                    Name = photographer.Name,
                    Biography = photographer.Biography,
                    BirthDate = photographer.BirthDate,
                    DeathDate = photographer.DeathDate,
                    PhotosCount = photographer.Photos?.Count ?? 0
                };
                return Ok(photographerDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Photographer>> Create(Photographer photographer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _photographerService.CreateAsync(photographer);
            var photographerDto = new PhotographerDTO()
            {
                Id = created.Id,
                Name = created.Name,
                Biography = created.Biography,
                BirthDate = created.BirthDate,
                DeathDate = created.DeathDate,
                PhotosCount = 0
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, photographerDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Photographer>> Update(int id, Photographer photographer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _photographerService.UpdateAsync(id, photographer);
                var photographerDto = new PhotographerDTO()
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    Biography = updated.Biography,
                    BirthDate = updated.BirthDate,
                    DeathDate = updated.DeathDate,
                    PhotosCount = updated.Photos?.Count ?? 0
                };
                return Ok(photographerDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _photographerService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Photographer>>> Search([FromQuery] string name)
        {
            var photographers = await _photographerService.SearchByNameAsync(name);
            var photographerDtos = photographers.Select(p => new PhotographerDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Biography = p.Biography,
                BirthDate = p.BirthDate,
                DeathDate = p.DeathDate,
                PhotosCount = p.Photos?.Count ?? 0
            });
            return Ok(photographerDtos);
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos(int id)
        {
            try
            {
                var photos = await _photographerService.GetPhotographerPhotosAsync(id);
                var photoDtos = photos.Select(p => new PhotoDTO
                {
                    Id = p.Id,
                    Url = p.Url,
                    Title = p.Title,
                    YearStart = p.YearStart,
                    YearEnd = p.YearEnd,
                    UserNote = p.UserNote,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    PhotographerId = p.PhotographerId,
                    PhotographerName = p.Photographer?.Name,
                    LocationId = p.LocationId,
                    SourceId = p.SourceId,
                    SourceName = p.Source.Collection,
                    UserId = p.UserId,
                    UserName = p.User.FirstName
                });
                return Ok(photoDtos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}