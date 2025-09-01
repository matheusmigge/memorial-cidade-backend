using Microsoft.AspNetCore.Mvc;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;

namespace memorial_cidade_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> GetAll()
        {
            var organizations = await _organizationService.GetAllAsync();
            return Ok(organizations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetById(int id)
        {
            try
            {
                var organization = await _organizationService.GetByIdAsync(id);
                return Ok(organization);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Organization>> Create(Organization organization)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _organizationService.CreateAsync(organization);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Organization>> Update(int id, Organization organization)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _organizationService.UpdateAsync(id, organization);
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
                await _organizationService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Organization>>> Search([FromQuery] string name)
        {
            var organizations = await _organizationService.SearchByNameAsync(name);
            return Ok(organizations);
        }
    }
}