using memorial_cidade_backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;

namespace memorial_cidade_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        private static PhotoDTO MapToPhotoDTO(Photo photo)
        {
            return new PhotoDTO
            {
                Id = photo.Id,
                Url = photo.Url,
                Title = photo.Title,
                YearStart = photo.YearStart,
                YearEnd = photo.YearEnd,
                UserNote = photo.UserNote,
                CreatedAt = photo.CreatedAt,
                UpdatedAt = photo.UpdatedAt,
                PhotographerId = photo.PhotographerId,
                PhotographerName = photo.Photographer?.Name,
                LocationId = photo.LocationData?.Id ?? 0,
                LocationData = photo.LocationData != null ? new LocationDataDTO
                {
                    Latitude = photo.LocationData.Latitude,
                    Longitude = photo.LocationData.Longitude,
                    Heading = photo.LocationData.Heading,
                    GoogleEarthPhotoUrl = photo.LocationData.GoogleEarthPhotoUrl,
                    GoogleEarthUrl = photo.LocationData.GoogleEarthUrl,
                    GoogleStreetViewEmbedUrl = photo.LocationData.GoogleStreetViewEmbedUrl
                } : null,
                SourceId = photo.SourceId,
                SourceName = photo.Source?.Collection,
                UserId = photo.UserId,
                UserName = (photo.User != null)
                    ? ($"{photo.User.FirstName} {photo.User.LastName}").Trim()
                    : null,
                TagNames = photo.Tags?.Select(t => t.Name).ToList() ?? new List<string>()
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhotoDTO>>> GetAll()
        {
            var photos = await _photoService.GetAllAsync();
            var photoDtos = photos.Select(MapToPhotoDTO).ToList();
            return Ok(photoDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Photo>> GetById(int id)
        {
            try
            {
                var photo = await _photoService.GetByIdAsync(id);
                return Ok(photo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Photo>> Create(CreatePhotoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var photo = new Photo
            {
                Url = dto.Url,
                Title = dto.Title,
                YearStart = dto.YearStart,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                PhotographerId = dto.PhotographerId,
                SourceId = dto.SourceId,
                UserId = dto.UserId,
                LocationData = dto.LocationData != null ? new LocationData
                {
                    Latitude = dto.LocationData.Latitude,
                    Longitude = dto.LocationData.Longitude,
                    Heading = dto.LocationData.Heading,
                    GoogleEarthPhotoUrl = dto.LocationData.GoogleEarthPhotoUrl,
                    GoogleEarthUrl = dto.LocationData.GoogleEarthUrl,
                    GoogleStreetViewEmbedUrl = dto.LocationData.GoogleStreetViewEmbedUrl
                } : null
            };

            var created = await _photoService.CreateAsync(photo);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Photo>> Update(int id, Photo photo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _photoService.UpdateAsync(id, photo);
                return Ok(updated);
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
                await _photoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("location/{locationId}")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetByLocation(int locationId)
        {
            var photos = await _photoService.GetByLocationIdAsync(locationId);
            return Ok(photos);
        }

        [HttpGet("photographer/{photographerId}")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetByPhotographer(int photographerId)
        {
            var photos = await _photoService.GetByPhotographerIdAsync(photographerId);
            return Ok(photos);
        }

        [HttpGet("source/{sourceId}")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetBySource(int sourceId)
        {
            var photos = await _photoService.GetBySourceIdAsync(sourceId);
            return Ok(photos);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetByUser(int userId)
        {
            var photos = await _photoService.GetByUserIdAsync(userId);
            return Ok(photos);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Photo>>> Search(
            [FromQuery] string? searchTerm,
            [FromQuery] int? yearStart,
            [FromQuery] int? yearEnd)
        {
            var photos = await _photoService.SearchAsync(searchTerm, yearStart, yearEnd);
            return Ok(photos);
        }

        [HttpPost("{photoId}/tags/{tagId}")]
        public async Task<ActionResult<Photo>> AddTag(int photoId, int tagId)
        {
            try
            {
                var photo = await _photoService.AddTagAsync(photoId, tagId);
                return Ok(photo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{photoId}/tags/{tagId}")]
        public async Task<ActionResult<Photo>> RemoveTag(int photoId, int tagId)
        {
            try
            {
                var photo = await _photoService.RemoveTagAsync(photoId, tagId);
                return Ok(photo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}