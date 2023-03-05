using AutoMapper;
using ForumAPI.Repositories.Models;

namespace ForumAPI.DTOs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Sections, opt => opt.MapFrom<CategoryToCategoryDtoResolver>());
            CreateMap<Section, SectionDto>()
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom<SectionToSectionDtoResolver>());
            CreateMap<Subject, SubjectDto>()
                .ForMember(dest => dest.Messages, opt => opt.MapFrom<SubjectToSubjectDtoResolver>());
            CreateMap<Message, MessageDto>();
        }
    }
}