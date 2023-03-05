using MySqlConnector;
using ForumAPI.Repositories.Models;
using ForumAPI.Repositories.Interfaces;

namespace ForumAPI.Repositories
{
    public class SubjectsRepository : IRepository<Subject>
    {
        private readonly IConfiguration _configuration;

        public SubjectsRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            tableName = "subjects";
        }

        override public Subject Map(MySqlDataReader reader)
        {
            Subject subject = new Subject();

            if (!reader.IsDBNull(reader.GetOrdinal("id")))
                subject.Id = reader.GetInt32(reader.GetOrdinal("id"));
            if (!reader.IsDBNull(reader.GetOrdinal("name")))
                subject.Name = reader.GetString(reader.GetOrdinal("name"));
            if (!reader.IsDBNull(reader.GetOrdinal("text")))
                subject.Text = reader.GetString(reader.GetOrdinal("text"));
            if (!reader.IsDBNull(reader.GetOrdinal("sectionId")))
                subject.SectionId = reader.GetInt32(reader.GetOrdinal("sectionId"));
            if (!reader.IsDBNull(reader.GetOrdinal("ownerId")))
                subject.OwnerId = reader.GetInt32(reader.GetOrdinal("ownerId"));

            return subject;
        }

        public override async Task<IEnumerable<Subject>> GetByParentIdAsync(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM " + tableName + " WHERE sectionId = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var subjects = new List<Subject>();

                        while (await reader.ReadAsync())
                        {
                            subjects.Add(Map(reader));
                        }

                        return subjects;
                    }
                }
            }
        }

        override public async Task<Subject> CreateAsync(Subject subject)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO subjects (name, sectionId, text, ownerId) VALUES (@name, @sectionId, @text, @ownerId);" +
                            "SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", subject.Name);
                    command.Parameters.AddWithValue("@sectionId", subject.SectionId);
                    command.Parameters.AddWithValue("@text", subject.Text);
                    command.Parameters.AddWithValue("@ownerId", subject.OwnerId);

                    var id = Convert.ToInt32(await command.ExecuteScalarAsync());

                    subject.Id = id;

                    return subject;
                }
            }
        }

        override public async Task<Subject> UpdateAsync(Subject subject)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE subjects SET name = @name, sectionId = @sectionId, text = @text WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", subject.Name);
                    command.Parameters.AddWithValue("@id", subject.Id);
                    command.Parameters.AddWithValue("@sectionId", subject.SectionId);
                    command.Parameters.AddWithValue("@text", subject.Text);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    return subject;
                }
            }
        }
    }
}
