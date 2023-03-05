using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;
using ForumAPI.Services.Interfaces;
using System.Security.Claims;

namespace ForumAPI.Services
{
    public class SubjectsService : IServiceBase<Subject>
    {
        private readonly IRepositoryBase<Subject> _subjectsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubjectsService(IRepositoryBase<Subject> subjectsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _subjectsRepository = subjectsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _subjectsRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Subject>> GetByParentIdAsync(int sectionId)
        {
            return await _subjectsRepository.GetByParentIdAsync(sectionId);
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            var existingSubject = await _subjectsRepository.GetByIdAsync(id);
            if (existingSubject == null)
            {
                throw new ArgumentException($"Subject with ID {id} not found.");
            }

            return existingSubject;
        }

        public async Task<Subject> CreateAsync(Subject subject)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                Console.WriteLine("UserId: " + userId);
                subject.OwnerId = int.Parse(userId);
            }
            return await _subjectsRepository.CreateAsync(subject);
        }

        public async Task<Subject> UpdateAsync(Subject subject)
        {
            var existingSubject = await _subjectsRepository.GetByIdAsync(subject.Id);
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (existingSubject == null)
            {
                throw new ArgumentException($"Subject with ID {subject.Id} not found.");
            }
            if (userId != existingSubject.OwnerId.ToString())
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");
            }

            existingSubject.Name = subject.Name;
            existingSubject.Text = subject.Text;
            existingSubject.SectionId = subject.SectionId;

            return await _subjectsRepository.UpdateAsync(existingSubject);
        }

        public async Task<bool> DeleteAsync(int subjectId)
        {
            var existingSubject = await _subjectsRepository.GetByIdAsync(subjectId);
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (existingSubject == null)
                throw new ArgumentException($"Subject with ID {subjectId} not found.");
            if (userId != existingSubject.OwnerId.ToString())
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");

            return await _subjectsRepository.DeleteAsync(subjectId);
        }
    }
}
