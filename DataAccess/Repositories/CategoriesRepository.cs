using MySqlConnector;
using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;

namespace ForumAPI.Repositories
{
    public class CategoriesRepository : IRepositoryBase<Category>
    {
        private readonly IConfiguration _configuration;

        public CategoriesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM categories";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var categories = new List<Category>();

                    while (await reader.ReadAsync())
                    {
                        Category category = new Category();

                        if (!reader.IsDBNull(reader.GetOrdinal("id")))
                            category.Id = reader.GetInt32(reader.GetOrdinal("id"));
                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            category.Name = reader.GetString(reader.GetOrdinal("name"));
                        if (!reader.IsDBNull(reader.GetOrdinal("description")))
                            category.Description = reader.GetString(reader.GetOrdinal("description"));
                        categories.Add(category);
                    }

                    return categories;
                }
            }
        }

        public Task<IEnumerable<Category>> GetByParentIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM categories WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Category category = new Category();

                            if (!reader.IsDBNull(reader.GetOrdinal("id")))
                                category.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            if (!reader.IsDBNull(reader.GetOrdinal("name")))
                                category.Name = reader.GetString(reader.GetOrdinal("name"));
                            if (!reader.IsDBNull(reader.GetOrdinal("description")))
                                category.Description = reader.GetString(reader.GetOrdinal("description"));

                            return category;
                        }

                        return null;
                    }
                }
            }
        }

        public async Task<Category> CreateAsync(Category category)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO categories (name) VALUES (@name);" +
                            "SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", category.Name);

                    var id = Convert.ToInt32(await command.ExecuteScalarAsync());

                    category.Id = id;

                    return category;
                }
            }
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE categories SET name = @name WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", category.Name);
                    command.Parameters.AddWithValue("@id", category.Id);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    return category;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM categories WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }
    }
}
