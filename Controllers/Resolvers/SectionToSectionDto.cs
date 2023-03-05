using AutoMapper;
using ForumAPI.Repositories.Models;
using ForumAPI.Services.Interfaces;

namespace ForumAPI.DTOs.Mapping
{
    public class SectionToSectionDtoResolver : IValueResolver<Section, SectionDto, List<SubjectDto>?>
    {
        private readonly IServiceBase<Subject> _subjectService;
        private readonly IMapper _mapper;

        public SectionToSectionDtoResolver(IServiceBase<Subject> subjectService, IMapper mapper)
        {
            _subjectService = subjectService;
            _mapper = mapper;
        }

        public List<SubjectDto> Resolve(Section source, SectionDto destination, List<SubjectDto>? destMember, ResolutionContext context)
        {
            var subjects = _subjectService.GetByParentIdAsync(source.Id).Result;
            var result = new List<SubjectDto>();
            foreach (var subject in subjects)
            {
                var subjectDto = _mapper.Map<SubjectDto>(subject);
                subjectDto.Messages = null;
                result.Add(subjectDto);
            }

            return result;
        }
    }
}