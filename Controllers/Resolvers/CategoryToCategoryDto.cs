using AutoMapper;
using ForumAPI.Repositories.Models;
using ForumAPI.Services.Interfaces;

namespace ForumAPI.DTOs.Mapping
{
    public class CategoryToCategoryDtoResolver : IValueResolver<Category, CategoryDto, List<SectionDto>?>
    {
        private readonly IServiceBase<Section> _sectionService;
        private readonly IMapper _mapper;

        public CategoryToCategoryDtoResolver(IServiceBase<Section> sectionService, IMapper mapper)
        {
            _sectionService = sectionService;
            _mapper = mapper;
        }

        public List<SectionDto> Resolve(Category source, CategoryDto destination, List<SectionDto>? destMember, ResolutionContext context)
        {
            var sections = _sectionService.GetByParentIdAsync(source.Id).Result;
            var result = new List<SectionDto>();
            Console.WriteLine("Sections: " + sections.Count());
            foreach (var section in sections)
            {
                var sectionDto = _mapper.Map<SectionDto>(section);
                sectionDto.Subjects = null;
                result.Add(sectionDto);
            }

            return result;
        }
    }
}