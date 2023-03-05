using AutoMapper;
using ForumAPI.Repositories.Models;
using ForumAPI.Services.Interfaces;

namespace ForumAPI.DTOs.Mapping
{
    public class UserResolver<TDestination> : IValueResolver<IOwnedModel, TDestination, UserDto>
    {
        private readonly IServiceBase<User> _userService;

        private readonly IMapper _mapper;

        public UserResolver(IServiceBase<User> userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public UserDto Resolve(IOwnedModel source, TDestination destination, UserDto destMember, ResolutionContext context)
        {
            var user = _userService.GetByIdAsync(source.OwnerId).Result;
            return _mapper.Map<UserDto>(user);
        }
    }
}