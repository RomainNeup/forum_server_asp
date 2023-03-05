using MySqlConnector;
using ForumAPI.Repositories.Models;

namespace ForumAPI.Repositories
{
    public class MessagesRepository : IRepository<Message>
    {
        private readonly IConfiguration _configuration;

        public MessagesRepository(IConfiguration configuration): base(configuration)
        {
            tableName = "messages";
            _configuration = configuration;
        }

        override public Message Map(MySqlDataReader reader)
        {
            Message message = new Message();

            if (!reader.IsDBNull(reader.GetOrdinal("id")))
                message.Id = reader.GetInt32(reader.GetOrdinal("id"));
            if (!reader.IsDBNull(reader.GetOrdinal("text")))
                message.Text = reader.GetString(reader.GetOrdinal("text"));
            if (!reader.IsDBNull(reader.GetOrdinal("subjectId")))
                message.SubjectId = reader.GetInt32(reader.GetOrdinal("subjectId"));
            if (!reader.IsDBNull(reader.GetOrdinal("ownerId")))
                message.OwnerId = reader.GetInt32(reader.GetOrdinal("ownerId"));

            return message;
        }

        override public async Task<IEnumerable<Message>> GetByParentIdAsync(int id) {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM " + tableName + " WHERE subjectId = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var messages = new List<Message>();

                        while (await reader.ReadAsync())
                        {
                            messages.Add(Map(reader));
                        }

                        return messages;
                    }
                }
            }
        }

        override public async Task<Message> CreateAsync(Message message)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO " + tableName +
                            " (text, subjectId, ownerId) VALUES (@text, @subjectId, @ownerId);" +
                            "SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@text", message.Text);
                    command.Parameters.AddWithValue("@subjectId", message.SubjectId);
                    command.Parameters.AddWithValue("@ownerId", message.OwnerId);

                    var id = Convert.ToInt32(await command.ExecuteScalarAsync());

                    message.Id = id;

                    return message;
                }
            }
        }

        override public async Task<Message> UpdateAsync(Message message)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE " + tableName + " SET text = @text, subjectId = @subjectId WHERE id = @id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@text", message.Text);
                    command.Parameters.AddWithValue("@subjectId", message.SubjectId);
                    command.Parameters.AddWithValue("@id", message.Id);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    return message;
                }
            }
        }
    }
}
