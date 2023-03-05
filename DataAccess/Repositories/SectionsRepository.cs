using MySqlConnector;
using ForumAPI.Repositories.Models;

namespace ForumAPI.Repositories
{
    public class SectionsRepository : IRepository<Section>
    {
        private readonly IConfiguration _configuration;

        public SectionsRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = "sections";
            _configuration = configuration;
        }

        override public Section Map(MySqlDataReader reader)
        {

            Section section = new Section();

            if (!reader.IsDBNull(reader.GetOrdinal("id")))
                section.Id = reader.GetInt32(reader.GetOrdinal("id"));
            if (!reader.IsDBNull(reader.GetOrdinal("name")))
                section.Name = reader.GetString(reader.GetOrdinal("name"));
            if (!reader.IsDBNull(reader.GetOrdinal("description")))
                section.Description = reader.GetString(reader.GetOrdinal("description"));
            if (!reader.IsDBNull(reader.GetOrdinal("categoryId")))
                section.CategoryId = reader.GetInt32(reader.GetOrdinal("categoryId"));
            if (!reader.IsDBNull(reader.GetOrdinal("ownerId")))
                section.OwnerId = reader.GetInt32(reader.GetOrdinal("ownerId"));

            return section;
        }

        override public async Task<IEnumerable<Section>> GetByParentIdAsync(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = $"SELECT * FROM {tableName} WHERE categoryId = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var sections = new List<Section>();

                        while (await reader.ReadAsync())
                        {
                            var section = Map(reader);

                            sections.Add(section);
                        }

                        return sections;
                    }
                }
            }
        }

        override public async Task<Section> CreateAsync(Section section)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO sections (name, description, categoryId, ownerId) VALUES (@name, @description, @categoryId, @ownerId);" +
                            "SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", section.Name);
                    command.Parameters.AddWithValue("@description", section.Description);
                    command.Parameters.AddWithValue("@categoryId", section.CategoryId);
                    command.Parameters.AddWithValue("@ownerId", section.OwnerId);

                    var id = Convert.ToInt32(await command.ExecuteScalarAsync());

                    section.Id = id;

                    return section;
                }
            }
        }

        override public async Task<Section> UpdateAsync(Section section)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE sections SET name = @name, description = @description, categoryId = @categoryId WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", section.Name);
                    command.Parameters.AddWithValue("@description", section.Description);
                    command.Parameters.AddWithValue("@categoryId", section.CategoryId);
                    command.Parameters.AddWithValue("@id", section.Id);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    return section;
                }
            }
        }
    }
}
