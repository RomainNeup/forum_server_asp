using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;
using ForumAPI.Services.Interfaces;
using System.Security.Claims;

namespace ForumAPI.Services
{
    public class SectionsService : IServiceBase<Section>
    {
        private readonly IRepositoryBase<Section> _sectionsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SectionsService(IRepositoryBase<Section> sectionsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sectionsRepository = sectionsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Section>> GetAllAsync()
        {
            return await _sectionsRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Section>> GetByParentIdAsync(int categoryId)
        {
            return await _sectionsRepository.GetByParentIdAsync(categoryId);
        }

        public async Task<Section> GetByIdAsync(int sectionId)
        {
            var existingSection = await _sectionsRepository.GetByIdAsync(sectionId);
            if (existingSection == null)
            {
                throw new ArgumentException($"Section with ID {sectionId} not found.");
            }

            return existingSection;
        }

        public async Task<Section> CreateAsync(Section section)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                section.OwnerId = int.Parse(userId);
            }

            return await _sectionsRepository.CreateAsync(section);
        }

        public async Task<Section> UpdateAsync(Section section)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingSection = await _sectionsRepository.GetByIdAsync(section.Id);

            if (existingSection == null)
            {
                throw new ArgumentException($"Section with ID {section.Id} not found.");
            }
            if (userId != existingSection.OwnerId.ToString())
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");
            }

            existingSection.Name = section.Name;
            existingSection.Description = section.Description;
            existingSection.CategoryId = section.CategoryId;

            return await _sectionsRepository.UpdateAsync(existingSection);
        }

        public async Task<bool> DeleteAsync(int sectionId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingSection = await _sectionsRepository.GetByIdAsync(sectionId);

            if (existingSection == null)
                throw new ArgumentException($"Section with ID {sectionId} not found.");
            if (userId != existingSection.OwnerId.ToString())
                throw new UnauthorizedAccessException("You are not authorized to edit this message.");

            return await _sectionsRepository.DeleteAsync(sectionId);
        }
    }
}
