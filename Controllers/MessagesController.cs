using Microsoft.AspNetCore.Mvc;
using ForumAPI.Services.Interfaces;
using ForumAPI.Repositories.Models;
using ForumAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("subjects/{subjectId}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IServiceBase<Message> _service;
        private readonly IMapper _mapper;

        public MessagesController(IServiceBase<Message> messageService, IMapper mapper)
        {
            _service = messageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<MessageDto>> Get([FromRoute] int subjectId)
        {
            var messages = await _service.GetByParentIdAsync(subjectId);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        [HttpGet("{id}")]
        public async Task<MessageDto> GetById(int id)
        {
            var messages = await _service.GetByIdAsync(id);
            return _mapper.Map<MessageDto>(messages);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Message>> Create(Message message, [FromRoute] int subjectId)
        {
            message.SubjectId = subjectId;
            await _service.CreateAsync(message);
            return CreatedAtAction(nameof(GetById), new { id = message.Id, subjectId = subjectId }, message);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, Message message)
        {
            message.Id = id;
            await _service.UpdateAsync(message);

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
