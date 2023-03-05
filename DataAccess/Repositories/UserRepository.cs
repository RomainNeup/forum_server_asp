using MySqlConnector;
using ForumAPI.Repositories.Models;

namespace ForumAPI.Repositories
{
    public class UserRepository: IRepository<User>
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration): base(configuration)
        {
            tableName = "users";
            _configuration = configuration;
        }

        override public User Map(MySqlDataReader reader)
        {
            User user = new User();

            if (!reader.IsDBNull(reader.GetOrdinal("id")))
                user.Id = reader.GetInt32(reader.GetOrdinal("id"));
            if (!reader.IsDBNull(reader.GetOrdinal("name")))
                user.Name = reader.GetString(reader.GetOrdinal("name"));
            if (!reader.IsDBNull(reader.GetOrdinal("email")))
                user.Email = reader.GetString(reader.GetOrdinal("email"));
            if (!reader.IsDBNull(reader.GetOrdinal("password")))
                user.Password = reader.GetString(reader.GetOrdinal("password"));
            if (!reader.IsDBNull(reader.GetOrdinal("role")))
                user.Role = reader.GetInt32(reader.GetOrdinal("role"));

            return user;
        }

        override public async Task<User> CreateAsync(User user)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = "INSERT INTO users (name, email, password, role) VALUES (@Name, @Email, @Password, @Role)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Role", user.Role);
                    await command.ExecuteNonQueryAsync();
                    return user;
                }
            }
        }

        public override Task<IEnumerable<User>> GetByParentIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        override public async Task<User> UpdateAsync(User user)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE users SET name = @Name, email = @Email, password = @Password WHERE id = @Id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    await command.ExecuteNonQueryAsync();

                    return user;
                }
            }
        }

        public async Task<User?> GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM users WHERE id = @Id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            User user = new User();

                            if (!reader.IsDBNull(reader.GetOrdinal("id")))
                                user.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            if (!reader.IsDBNull(reader.GetOrdinal("name")))
                                user.Name = reader.GetString(reader.GetOrdinal("name"));
                            if (!reader.IsDBNull(reader.GetOrdinal("email")))
                                user.Email = reader.GetString(reader.GetOrdinal("email"));
                            if (!reader.IsDBNull(reader.GetOrdinal("role")))
                                user.Role = reader.GetInt32(reader.GetOrdinal("role"));

                            return user;
                        }

                        return null;
                    }
                }
            }
        }

        public async Task<User?> GetByEmail(string email)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM users WHERE email = @Email";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            User user = new User();

                            if (!reader.IsDBNull(reader.GetOrdinal("id")))
                                user.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            if (!reader.IsDBNull(reader.GetOrdinal("name")))
                                user.Name = reader.GetString(reader.GetOrdinal("name"));
                            if (!reader.IsDBNull(reader.GetOrdinal("email")))
                                user.Email = reader.GetString(reader.GetOrdinal("email"));
                            if (!reader.IsDBNull(reader.GetOrdinal("role")))
                                user.Role = reader.GetInt32(reader.GetOrdinal("role"));

                            return user;
                        }

                        return null;
                    }
                }
            }
        }

    }
}