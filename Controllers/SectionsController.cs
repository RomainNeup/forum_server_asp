using Microsoft.AspNetCore.Mvc;
using ForumAPI.Services.Interfaces;
using ForumAPI.Repositories.Models;
using ForumAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly IServiceBase<Section> _service;
        private readonly IMapper _mapper;

        public SectionsController(IServiceBase<Section> sectionService, IMapper mapper)
        {
            _service = sectionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<SectionDto>> Get([FromQuery] string? fields)
        {
            var sections = await _service.GetAllAsync();
            var sectionsDto = _mapper.Map<IEnumerable<SectionDto>>(sections);
            if (fields == null || !fields.ToLower().Split(',').Contains("subjects"))
            {
                foreach (var section in sectionsDto)
                {
                    section.Subjects = null;
                }
            }
            return sectionsDto;
        }

        [HttpGet("{id}")]
        public async Task<SectionDto> GetById(int id)
        {
            var section = await _service.GetByIdAsync(id);
            var sectionDto = _mapper.Map<SectionDto>(section);
            return sectionDto;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Section>> Create(Section section)
        {
            await _service.CreateAsync(section);
            return CreatedAtAction(nameof(GetById), new { id = section.Id }, section);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, Section section)
        {
            section.Id = id;
            await _service.UpdateAsync(section);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
