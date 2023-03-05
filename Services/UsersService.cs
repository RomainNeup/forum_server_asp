using ForumAPI.Repositories.Models;
using ForumAPI.Repositories;
using ForumAPI.Services.Interfaces;
using System.Security.Claims;

namespace ForumAPI.Services
{
    public class UsersService : IServiceBase<User>
    {
        private readonly UserRepository _usersRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(UserRepository usersRepository, IHttpContextAccessor httpContextAccessor)
        {
            _usersRepository = usersRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _usersRepository.GetAllAsync();
        }

        public Task<IEnumerable<User>> GetByParentIdAsync(int sectionId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var existingUser = await _usersRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new ArgumentException($"User with ID {id} not found.");
            }

            return existingUser;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var existingUser = await _usersRepository.GetByEmail(email);
            if (existingUser == null)
            {
                throw new ArgumentException($"User with email {email} not found.");
            }

            return existingUser;
        }

        public async Task<User> CreateAsync(User user)
        {
            return await _usersRepository.CreateAsync(user);
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await _usersRepository.GetByIdAsync(user.Id);
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (existingUser == null)
            {
                throw new ArgumentException($"User with ID {user.Id} not found.");
            }
            if (userId != existingUser.Id.ToString())
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;

            return await _usersRepository.UpdateAsync(existingUser);
        }

        public async Task<bool> DeleteAsync(int _)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");
            var existingUser = await _usersRepository.GetByIdAsync(int.Parse(userId));

            if (existingUser == null)
                throw new ArgumentException($"User with ID {userId} not found.");
            if (userId != existingUser.Id.ToString())
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");

            return await _usersRepository.DeleteAsync(int.Parse(userId));
        }
    }
}
