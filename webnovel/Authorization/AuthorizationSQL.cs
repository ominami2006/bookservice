using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
namespace bookservice {
    public class AuthorizationSQL : IDisposable {
        private Connection dbConnection;
        private User user;
        public AuthorizationSQL() {
            dbConnection = new Connection();
            this.user = new User();
            updateUser(user);
        }
        public AuthorizationSQL(User user) {
            dbConnection = new Connection();
            updateUser(user);
        }
        public void updateUser(User user) {
            this.user = user;
            string connectionString = "";
            switch (this.user.Role) {
                case UserRole.WN_GUEST:
                    connectionString = "Data Source=localhost;Initial Catalog=webnovel;User ID=WN_GUEST;Password=guest;";
                    break;
                case UserRole.WN_READER:
                    connectionString = "Data Source=localhost;Initial Catalog=webnovel;User ID=WN_READER;Password=reader;";
                    break;
                case UserRole.WN_WRITER:
                    connectionString = "Data Source=localhost;Initial Catalog=webnovel;User ID=WN_WRITER;Password=writer;";
                    break;
                default:
                    connectionString = "Data Source=localhost;Initial Catalog=webnovel;User ID=WN_GUEST;Password=guest;";
                    break;
            }
            dbConnection.SetСonnectionString(connectionString);
        }
        public string HashPassword(string password) {
            using (SHA256 sha256Hash = SHA256.Create()) {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }
        public User CheckLogin(string login, string plainPassword) {
            User loggedInUser = null;
            string hashedPassword = HashPassword(plainPassword);
            try {
                dbConnection.OpenConnection();
                using (SqlCommand cmd = new SqlCommand("checkUserLogin", dbConnection.GetConnection())) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            loggedInUser = new User {
                                Id = Convert.ToInt32(reader["id"]),
                                Login = reader["login"].ToString(),
                                IsWriter = Convert.ToBoolean(reader["is_writer"]),
                                HashedPassword = hashedPassword
                            };
                        }
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine($"Ошибка SQL при авторизации: {ex.Message}");
            } catch (Exception ex) {
                Console.WriteLine($"Общая ошибка при авторизации: {ex.Message}");
            } finally {
                dbConnection.CloseConnection();
            }
            return loggedInUser;
        }
        public User RegisterUser(string login, string plainPassword, bool isWriter) {
            User newUser = null;
            string hashedPassword = HashPassword(plainPassword);
            try {
                dbConnection.OpenConnection();
                using (SqlCommand cmd = new SqlCommand("registerNewUser", dbConnection.GetConnection())) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                    cmd.Parameters.AddWithValue("@IsWriter", isWriter);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            int newUserId = Convert.ToInt32(reader["NewUserID"]);
                            string message = reader["Message"].ToString();
                            if (newUserId != -1 && message.Equals("Success", StringComparison.OrdinalIgnoreCase)) {
                                newUser = new User {
                                    Id = newUserId,
                                    Login = login,
                                    IsWriter = isWriter,
                                    HashedPassword = hashedPassword
                                };
                            } else {
                                Console.WriteLine($"Ошибка регистрации: {message}");
                            }
                        }
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine($"Ошибка SQL при регистрации: {ex.Message}");
            } catch (Exception ex) {
                Console.WriteLine($"Общая ошибка при регистрации: {ex.Message}");
            } finally {
                dbConnection.CloseConnection();
            }
            return newUser;
        }
        public bool BecomeWriter(int userId) {
            bool success = false;
            try {
                dbConnection.OpenConnection();
                using (SqlCommand cmd = new SqlCommand("UpdateUserWriterStatus", dbConnection.GetConnection())) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@NewIsWriterStatus", true);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            int rowsAffected = Convert.ToInt32(reader["RowsAffected"]);
                            string statusMessage = reader["StatusMessage"].ToString();
                            if (rowsAffected > 0 && statusMessage.Equals("Success", StringComparison.OrdinalIgnoreCase)) {
                                success = true;
                            } else {
                                Console.WriteLine($"Не удалось обновить статус: {statusMessage}");
                            }
                        }
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine($"Ошибка SQL при обновлении статуса: {ex.Message}");
            } catch (Exception ex) {
                Console.WriteLine($"Общая ошибка при обновлении статуса: {ex.Message}");
            } finally {
                dbConnection.CloseConnection();
            }
            return success;
        }
        public void Dispose() {
            if (dbConnection != null) {
                dbConnection.Dispose();
                dbConnection = null;
            }
            GC.SuppressFinalize(this);
        }
        ~AuthorizationSQL() {
            Dispose();
        }
    }
}
