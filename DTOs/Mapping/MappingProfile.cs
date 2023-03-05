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
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom<SectionToSectionDtoResolver>())
                .ForMember(dest => dest.Owner, opt => opt.MapFrom<UserResolver<SectionDto>>());
            CreateMap<Subject, SubjectDto>()
                .ForMember(dest => dest.Messages, opt => opt.MapFrom<SubjectToSubjectDtoResolver>())
                .ForMember(dest => dest.Owner, opt => opt.MapFrom<UserResolver<SubjectDto>>());
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom<UserResolver<MessageDto>>());
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.Ignore());
        }
    }
}