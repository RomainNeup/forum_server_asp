using Microsoft.AspNetCore.Mvc;
using ForumAPI.Services.Interfaces;
using ForumAPI.Repositories.Models;
using ForumAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("sections/{sectionId}/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly IServiceBase<Subject> _service;
        private readonly IMapper _mapper;

        public SubjectsController(IServiceBase<Subject> subjectService, IMapper mapper)
        {
            _service = subjectService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<SubjectDto>> Get([FromRoute] int sectionId, [FromQuery] string? fields)
        {
            var subjects = await _service.GetByParentIdAsync(sectionId);
            var subjectsDto = _mapper.Map<IEnumerable<SubjectDto>>(subjects);
            if (fields == null || !fields.ToLower().Split(',').Contains("messages"))
            {
                foreach (var subject in subjectsDto)
                {
                    subject.Messages = null;
                }
            }
            return subjectsDto;
        }

        [HttpGet("{id}")]
        public async Task<SubjectDto> GetById(int id)
        {
            var subject = await _service.GetByIdAsync(id);
            var subjectDto = _mapper.Map<SubjectDto>(subject);
            return subjectDto;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Subject>> Create(Subject subject, [FromRoute] int sectionId)
        {
            subject.SectionId = sectionId;
            await _service.CreateAsync(subject);
            Console.WriteLine(subject.Id);
            return CreatedAtAction(nameof(GetById), new { sectionId = sectionId, id = subject.Id }, subject);

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, Subject subject)
        {
            subject.Id = id;
            await _service.UpdateAsync(subject);

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
