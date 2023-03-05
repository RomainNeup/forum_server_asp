using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;
using ForumAPI.Services.Interfaces;

namespace ForumAPI.Services
{
    public class SectionsService : IServiceBase<Section>
    {
        private readonly IRepositoryBase<Section> _sectionsRepository;

        public SectionsService(IRepositoryBase<Section> sectionsRepository)
        {
            _sectionsRepository = sectionsRepository;
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
            return await _sectionsRepository.CreateAsync(section);
        }

        public async Task<Section> UpdateAsync(Section section)
        {
            var existingSection = await _sectionsRepository.GetByIdAsync(section.Id);
            if (existingSection == null)
            {
                throw new ArgumentException($"Section with ID {section.Id} not found.");
            }

            existingSection.Name = section.Name;
            existingSection.Description = section.Description;
            existingSection.CategoryId = section.CategoryId;

            return await _sectionsRepository.UpdateAsync(existingSection);
        }

        public async Task<bool> DeleteAsync(int sectionId)
        {
            if (await _sectionsRepository.GetByIdAsync(sectionId) == null)
                throw new ArgumentException($"Section with ID {sectionId} not found.");
            return await _sectionsRepository.DeleteAsync(sectionId);
        }
    }
}
