using Microsoft.AspNetCore.Mvc;
using memorial_cidade_backend.Models;
using memorial_cidade_backend.Services.Interfaces;

namespace memorial_cidade_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagCategoryController : ControllerBase
    {
        private readonly ITagCategoryService _tagCategoryService;

        public TagCategoryController(ITagCategoryService tagCategoryService)
        {
            _tagCategoryService = tagCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagCategory>>> GetAll()
        {
            var categories = await _tagCategoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagCategory>> GetById(int id)
        {
            try
            {
                var category = await _tagCategoryService.GetByIdAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TagCategory>> Create(TagCategory tagCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _tagCategoryService.CreateAsync(tagCategory);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TagCategory>> Update(int id, TagCategory tagCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _tagCategoryService.UpdateAsync(id, tagCategory);
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
                await _tagCategoryService.DeleteAsync(id);
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
        public async Task<ActionResult<IEnumerable<TagCategory>>> Search([FromQuery] string name)
        {
            var categories = await _tagCategoryService.SearchByNameAsync(name);
            return Ok(categories);
        }

        [HttpGet("{id}/tags")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags(int id)
        {
            try
            {
                var tags = await _tagCategoryService.GetCategoryTagsAsync(id);
                return Ok(tags);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}