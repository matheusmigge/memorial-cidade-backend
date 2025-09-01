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
            return Ok(photographers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Photographer>> GetById(int id)
        {
            try
            {
                var photographer = await _photographerService.GetByIdAsync(id);
                return Ok(photographer);
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
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Photographer>> Update(int id, Photographer photographer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _photographerService.UpdateAsync(id, photographer);
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
            return Ok(photographers);
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos(int id)
        {
            try
            {
                var photos = await _photographerService.GetPhotographerPhotosAsync(id);
                return Ok(photos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}