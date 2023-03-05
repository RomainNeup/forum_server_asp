using MySqlConnector;
using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;

namespace ForumAPI.Repositories
{
    public abstract class IRepository<T> : IRepositoryBase<T> where T : class
    {
        private readonly IConfiguration _configuration;
        protected String tableName = "";

        public IRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public abstract T Map(MySqlDataReader reader);

        public abstract Task<IEnumerable<T>> GetByParentIdAsync(int id);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM " + tableName;

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var sections = new List<T>();

                    while (await reader.ReadAsync())
                    {
                        sections.Add(Map(reader));
                    }

                    return sections;
                }
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM " + tableName + " WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return Map(reader);
                        }

                        return null;
                    }
                }
            }
        }

        public abstract Task<T> CreateAsync(T section);

        public abstract Task<T> UpdateAsync(T section);

        public async Task<bool> DeleteAsync(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM " + tableName + " WHERE id = @id";

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
