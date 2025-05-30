using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace bookservice
{
    public class UserBookHistoryEntry
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public bool IsFinished { get; set; }
        public int? CurrentChapterNumber { get; set; }
        public int? CurrentPageNumber { get; set; }
        public DateTime LastAccessedDate { get; set; }

        public string StatusDisplay
        {
            get
            {
                if (IsFinished)
                {
                    return "Прочитано";
                }
                else if (CurrentChapterNumber.HasValue && CurrentPageNumber.HasValue)
                {
                    return $"В процессе: Глава {CurrentChapterNumber}, Стр. {CurrentPageNumber}";
                }
                return "Статус неизвестен";
            }
        }
    }

    public class HistorySQL : IDisposable
    {
        private Connection dbConnection;
        private User user;

        public HistorySQL(User currentUser)
        {
            dbConnection = new Connection();
            UpdateUser(currentUser);
        }

        public void UpdateUser(User currentUser)
        {
            this.user = currentUser;
            string connectionString = "";
            // Assuming history can be viewed by any logged-in user role with their own permissions
            // If specific roles are needed, adjust connection string logic.
            // For now, using reader for logged-in users, guest for non-logged-in.
            if (this.user != null && this.user.Id != 0) // Any logged-in user
            {
                // Use admin connection for now, as it has broader read access potentially
                // Or, ensure 'reader' role has rights to select from user_books and books.
                // For simplicity, using admin here, but 'reader' should be preferred if permissions allow.
                switch (this.user.Role)
                {
                    case UserRole.READER:
                        connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=reader;Password=reader;";
                        break;
                    case UserRole.ADMIN:
                        connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=admin;Password=admin;";
                        break;
                    default: // Should not happen for logged in user, but as fallback
                        connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=guest;Password=guest;";
                        break;
                }
            }
            else // Guest
            {
                connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=guest;Password=guest;";
            }
            dbConnection.SetСonnectionString(connectionString);
        }

        public List<UserBookHistoryEntry> GetUserReadingHistory(int userId)
        {
            List<UserBookHistoryEntry> history = new List<UserBookHistoryEntry>();
            if (userId == 0) return history; // No history for guests

            try
            {
                dbConnection.OpenConnection();
                string query = @"
                    SELECT 
                        ub.book_id, 
                        b.title AS book_title, 
                        ub.status, 
                        ub.current_chapter_number, 
                        ub.current_page_number, 
                        ub.last_accessed_date 
                    FROM user_books ub
                    JOIN books b ON ub.book_id = b.id
                    WHERE ub.user_id = @userId
                    ORDER BY ub.last_accessed_date DESC";

                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new UserBookHistoryEntry
                            {
                                BookId = Convert.ToInt32(reader["book_id"]),
                                BookTitle = reader["book_title"].ToString(),
                                IsFinished = Convert.ToBoolean(reader["status"]),
                                CurrentChapterNumber = reader["current_chapter_number"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["current_chapter_number"]),
                                CurrentPageNumber = reader["current_page_number"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["current_page_number"]),
                                LastAccessedDate = Convert.ToDateTime(reader["last_accessed_date"])
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error getting user reading history: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting user reading history: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return history;
        }

        public void Dispose()
        {
            if (dbConnection != null)
            {
                dbConnection.Dispose();
                dbConnection = null;
            }
        }
    }
}