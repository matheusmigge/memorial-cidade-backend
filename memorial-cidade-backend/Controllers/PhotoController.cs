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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photo>>> GetAll()
        {
            var photos = await _photoService.GetAllAsync();
            return Ok(photos);
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
        public async Task<ActionResult<Photo>> Create(Photo photo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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