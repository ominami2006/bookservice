﻿using System;
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
                case UserRole.GUEST: // Renamed
                    connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=guest;Password=guest;"; // User renamed
                    break;
                case UserRole.READER: // Renamed
                    connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=reader;Password=reader;"; // User renamed
                    break;
                case UserRole.ADMIN:  // Renamed
                    connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=admin;Password=admin;"; // User renamed
                    break;
                default:
                    connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=guest;Password=guest;"; // User renamed
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
                // Stored procedure 'checkUserLogin' was updated to return 'is_admin'
                using (SqlCommand cmd = new SqlCommand("checkUserLogin", dbConnection.GetConnection())) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            loggedInUser = new User {
                                Id = Convert.ToInt32(reader["id"]),
                                Login = reader["login"].ToString(),
                                IsAdmin = Convert.ToBoolean(reader["is_admin"]), // Changed from is_writer
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
        public User RegisterUser(string login, string plainPassword, bool isAdmin) { // Changed from isWriter to isAdmin
            User newUser = null;
            string hashedPassword = HashPassword(plainPassword);
            try {
                dbConnection.OpenConnection();
                // Stored procedure 'registerNewUser' was updated for 'is_admin'
                using (SqlCommand cmd = new SqlCommand("registerNewUser", dbConnection.GetConnection())) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                    cmd.Parameters.AddWithValue("@IsAdmin", isAdmin); // Changed from @IsWriter
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            int newUserId = Convert.ToInt32(reader["NewUserID"]);
                            string message = reader["Message"].ToString();
                            if (newUserId != -1 && message.Equals("Success", StringComparison.OrdinalIgnoreCase)) {
                                newUser = new User {
                                    Id = newUserId,
                                    Login = login,
                                    IsAdmin = isAdmin, // Changed from isWriter
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
        // Removed BecomeWriter method as 'updateUserWriterStatus' procedure was removed.
        // Admin assignment is handled differently now.
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
