using AutoMapper;
using ForumAPI.Repositories.Models;
using ForumAPI.Services.Interfaces;

namespace ForumAPI.DTOs.Mapping
{
    public class SubjectToSubjectDtoResolver : IValueResolver<Subject, SubjectDto, List<MessageDto>?>
    {
        private readonly IServiceBase<Message> _messageService;
        private readonly IMapper _mapper;

        public SubjectToSubjectDtoResolver(IServiceBase<Message> messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        public List<MessageDto> Resolve(Subject source, SubjectDto destination, List<MessageDto>? destMember, ResolutionContext context)
        {
            var messages = _messageService.GetByParentIdAsync(source.Id).Result;
            var result = new List<MessageDto>();
            foreach (var message in messages)
            {
                var messageDto = _mapper.Map<MessageDto>(message);
                result.Add(messageDto);
            }

            return result;
        }
    }
}