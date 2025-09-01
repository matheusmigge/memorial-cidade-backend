using Microsoft.AspNetCore.Mvc;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;

namespace memorial_cidade_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SourceController : ControllerBase
    {
        private readonly ISourceService _sourceService;

        public SourceController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Source>>> GetAll()
        {
            var sources = await _sourceService.GetAllAsync();
            return Ok(sources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Source>> GetById(int id)
        {
            try
            {
                var source = await _sourceService.GetByIdAsync(id);
                return Ok(source);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Source>> Create(Source source)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _sourceService.CreateAsync(source);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Source>> Update(int id, Source source)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _sourceService.UpdateAsync(id, source);
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
                await _sourceService.DeleteAsync(id);
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

        [HttpGet("organization/{organizationId}")]
        public async Task<ActionResult<IEnumerable<Source>>> GetByOrganization(int organizationId)
        {
            var sources = await _sourceService.GetByOrganizationIdAsync(organizationId);
            return Ok(sources);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Source>>> SearchByCollection([FromQuery] string collection)
        {
            var sources = await _sourceService.SearchByCollectionAsync(collection);
            return Ok(sources);
        }
    }
}