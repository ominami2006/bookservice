using System;
using System.Data;
using System.Data.SqlClient;

namespace bookservice
{
    public class ReadingProgress
    {
        public int ChapterNumber { get; set; }
        public int PageNumber { get; set; }
        public bool IsFinished { get; set; }
        public bool Exists { get; set; } // To indicate if a record was found

        public ReadingProgress()
        {
            Exists = false;
            IsFinished = false;
            ChapterNumber = 1; // Default to start
            PageNumber = 1;    // Default to start
        }
    }

    public class ReadingSQL : IDisposable
    {
        private Connection dbConnection;
        private User user;

        public ReadingSQL(User currentUser)
        {
            dbConnection = new Connection();
            UpdateUser(currentUser);
        }

        public void UpdateUser(User currentUser)
        {
            this.user = currentUser;
            string connectionString = "";
            switch (this.user.Role)
            {
                case UserRole.GUEST:
                    connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=guest;Password=guest;";
                    break;
                case UserRole.READER:
                    connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=reader;Password=reader;";
                    break;
                case UserRole.ADMIN:
                    connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=admin;Password=admin;";
                    break;
                default:
                    connectionString = "Data Source=localhost;Initial Catalog=bookservice;User ID=guest;Password=guest;";
                    break;
            }
            dbConnection.SetСonnectionString(connectionString);
        }

        public ReadingProgress GetReadingProgress(int userId, int bookId)
        {
            ReadingProgress progress = new ReadingProgress();
            if (userId == 0) return progress; // Guests don't have saved progress

            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT current_chapter_number, current_page_number, status FROM user_books WHERE user_id = @userId AND book_id = @bookId";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@bookId", bookId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            progress.Exists = true;
                            progress.ChapterNumber = reader["current_chapter_number"] == DBNull.Value ? 1 : Convert.ToInt32(reader["current_chapter_number"]);
                            progress.PageNumber = reader["current_page_number"] == DBNull.Value ? 1 : Convert.ToInt32(reader["current_page_number"]);
                            progress.IsFinished = Convert.ToBoolean(reader["status"]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error getting reading progress: {ex.Message}");
                // Return default progress, so reading can start from beginning
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error getting reading progress: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return progress;
        }

        public bool SaveReadingProgress(int userId, int bookId, int chapterNumber, int pageNumber, bool isFinished)
        {
            if (userId == 0) return false; // Cannot save progress for guests

            try
            {
                dbConnection.OpenConnection();
                string checkQuery = "SELECT COUNT(*) FROM user_books WHERE user_id = @userId AND book_id = @bookId";
                SqlCommand cmd;
                int recordExists = 0;

                using (cmd = new SqlCommand(checkQuery, dbConnection.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    recordExists = (int)cmd.ExecuteScalar();
                }

                if (recordExists > 0)
                {
                    // Update existing record
                    string updateQuery = @"
                        UPDATE user_books 
                        SET current_chapter_number = @chapterNumber, 
                            current_page_number = @pageNumber, 
                            status = @status, 
                            last_accessed_date = @lastAccessedDate 
                        WHERE user_id = @userId AND book_id = @bookId";
                    cmd = new SqlCommand(updateQuery, dbConnection.GetConnection());
                }
                else
                {
                    // Insert new record
                    string insertQuery = @"
                        INSERT INTO user_books (user_id, book_id, current_chapter_number, current_page_number, status, last_accessed_date) 
                        VALUES (@userId, @bookId, @chapterNumber, @pageNumber, @status, @lastAccessedDate)";
                    cmd = new SqlCommand(insertQuery, dbConnection.GetConnection());
                }

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@bookId", bookId);
                cmd.Parameters.AddWithValue("@chapterNumber", chapterNumber);
                cmd.Parameters.AddWithValue("@pageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@status", isFinished);
                cmd.Parameters.AddWithValue("@lastAccessedDate", DateTime.Now);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error saving reading progress: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error saving reading progress: {ex.Message}");
                return false;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
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