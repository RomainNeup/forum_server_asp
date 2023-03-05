using Microsoft.AspNetCore.Mvc;
using ForumAPI.Services.Interfaces;
using ForumAPI.Repositories.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ForumAPI.DTOs;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceBase<Category> _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(IServiceBase<Category> categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> Get([FromQuery] string? fields)
        {
            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            if (fields == null || !fields.ToLower().Split(',').Contains("sections"))
            {
                foreach (var category in categoriesDto)
                {
                    category.Sections = null;
                }
            }
            return categoriesDto;
        }

        [HttpGet("{id}")]
        public async Task<CategoryDto> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            await _categoryService.CreateAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            category.Id = id;
            await _categoryService.UpdateAsync(category);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);

            return NoContent();
        }
    }
}
