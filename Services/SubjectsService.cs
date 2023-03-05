using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;
using ForumAPI.Services.Interfaces;

namespace ForumAPI.Services
{
    public class SubjectsService : IServiceBase<Subject>
    {
        private readonly IRepositoryBase<Subject> _subjectsRepository;

        public SubjectsService(IRepositoryBase<Subject> subjectsRepository)
        {
            _subjectsRepository = subjectsRepository;
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
            return await _subjectsRepository.CreateAsync(subject);
        }

        public async Task<Subject> UpdateAsync(Subject subject)
        {
            var existingSubject = await _subjectsRepository.GetByIdAsync(subject.Id);
            if (existingSubject == null)
            {
                throw new ArgumentException($"Subject with ID {subject.Id} not found.");
            }

            existingSubject.Name = subject.Name;
            existingSubject.Text = subject.Text;
            existingSubject.SectionId = subject.SectionId;

            return await _subjectsRepository.UpdateAsync(existingSubject);
        }

        public async Task<bool> DeleteAsync(int subjectId)
        {
            if (await _subjectsRepository.GetByIdAsync(subjectId) == null)
                throw new ArgumentException($"Subject with ID {subjectId} not found.");
            return await _subjectsRepository.DeleteAsync(subjectId);
        }
    }
}
