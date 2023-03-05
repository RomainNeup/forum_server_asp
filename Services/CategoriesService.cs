using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;
using ForumAPI.Services.Interfaces;

namespace ForumAPI.Services
{
    public class CategoriesService : IServiceBase<Category>
    {
        private readonly IRepositoryBase<Category> _categoriesRepository;

        public CategoriesService(IRepositoryBase<Category> categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoriesRepository.GetAllAsync();
        }

        public Task<IEnumerable<Category>> GetByParentIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetByIdAsync(int categoryId)
        {
            var existingCategory = await _categoriesRepository.GetByIdAsync(categoryId);
            if (existingCategory == null)
            {
                throw new ArgumentException($"Category with ID {categoryId} not found.");
            }

            return existingCategory;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            return await _categoriesRepository.CreateAsync(category);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = await _categoriesRepository.GetByIdAsync(category.Id);
            if (existingCategory == null)
            {
                throw new ArgumentException($"Category with ID {category.Id} not found.");
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            return await _categoriesRepository.UpdateAsync(existingCategory);
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            if (await _categoriesRepository.GetByIdAsync(categoryId) == null)
                throw new ArgumentException($"Category with ID {categoryId} not found.");
            return await _categoriesRepository.DeleteAsync(categoryId);
        }
    }
}
