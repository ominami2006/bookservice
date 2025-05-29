﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace bookservice {
    // Renamed from NovelDetails
    public class BookDetails {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AdminId { get; set; } // Renamed from WriterId
        public string AdminLogin { get; set; } // Renamed from WriterLogin
        public string WriterName { get; set; } // New field
        public short? PublicationYear { get; set; }
        public short? AgeRating { get; set; }
        public string Description { get; set; }
        public string CoverPath { get; set; }
    }
    // Renamed from NovelFile
    public class BookFile {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; } // Path now points to .fb2
        public DateTime? PublicationDate { get; set; }
    }
    // Renamed from NovelComment
    public class BookComment {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string Text { get; set; }
        public DateTime CommentDateTime { get; set; }
    }
    public class BookSQL : IDisposable {
        private Connection dbConnection;
        private User user;
        public BookSQL() {
            dbConnection = new Connection();
            this.user = new User();
            UpdateUser(user);
        }
        public BookSQL(User user) {
            dbConnection = new Connection();
            UpdateUser(user);
        }
        public void UpdateUser(User user) {
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
        public string GetUserLoginById(int userId) {
            string login = "Неизвестный пользователь";
            try {
                dbConnection.OpenConnection();
                using (SqlCommand cmd = new SqlCommand("getUserLoginById", dbConnection.GetConnection())) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        login = result.ToString();
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении логина пользователя: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении логина пользователя: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return login;
        }
        // Renamed from GetNovelDetails
        public BookDetails GetBookDetails(int bookId) {
            BookDetails details = null;
            try {
                dbConnection.OpenConnection();
                // Query 'books' table, select 'admin_id' and 'writer_name'
                string query = "SELECT id, title, admin_id, writer_name, publication_year, age_rating, description, cover_path FROM books WHERE id = @bookId";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            details = new BookDetails {
                                Id = Convert.ToInt32(reader["id"]),
                                Title = reader["title"].ToString(),
                                AdminId = Convert.ToInt32(reader["admin_id"]), // Changed from writer_id
                                WriterName = reader["writer_name"].ToString(), // New field
                                PublicationYear = reader["publication_year"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["publication_year"]),
                                AgeRating = reader["age_rating"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["age_rating"]),
                                Description = reader["description"].ToString(),
                                CoverPath = reader["cover_path"].ToString()
                            };
                        }
                    }
                }
                if (details != null)
                    details.AdminLogin = GetUserLoginById(details.AdminId); // Get login of the admin who added the book
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении деталей книги: " + ex.Message); // Updated message
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении деталей книги: " + ex.Message); // Updated message
            } finally {
                dbConnection.CloseConnection();
            }
            return details;
        }
        // Renamed from GetNovelGenres
        public List<string> GetBookGenres(int bookId) {
            List<string> genres = new List<string>();
            try {
                dbConnection.OpenConnection();
                string query = @"
                    SELECT g.genre_name
                    FROM genres g
                    INNER JOIN book_genres bg ON g.id = bg.genre_id 
                    WHERE bg.book_id = @bookId 
                    ORDER BY g.genre_name ASC"; // Renamed webnovel_genres to book_genres, wg to bg, webnovel_id to book_id
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@bookId", bookId); // Renamed parameter
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                            genres.Add(reader["genre_name"].ToString());
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении жанров книги: " + ex.Message); // Updated message
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении жанров книги: " + ex.Message); // Updated message
            } finally {
                dbConnection.CloseConnection();
            }
            return genres;
        }
        // Renamed from GetNovelFiles
        public List<BookFile> GetBookFiles(int bookId) {
            List<BookFile> files = new List<BookFile>();
            try {
                dbConnection.OpenConnection();
                // Query 'book_files' table
                string query = "SELECT id, file_name, file_path, publication_date FROM book_files WHERE book_id = @bookId ORDER BY file_name ASC";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@bookId", bookId); // Renamed parameter and column book_id
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            files.Add(new BookFile { // Use BookFile
                                Id = Convert.ToInt32(reader["id"]),
                                FileName = reader["file_name"].ToString(),
                                FilePath = reader["file_path"].ToString(), // FilePath now refers to .fb2
                                PublicationDate = reader["publication_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["publication_date"])
                            });
                        }
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении файлов книги: " + ex.Message); // Updated message
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении файлов книги: " + ex.Message); // Updated message
            } finally {
                dbConnection.CloseConnection();
            }
            return files;
        }
        // Renamed from GetNovelComments
        public List<BookComment> GetBookComments(int bookId) {
            List<BookComment> comments = new List<BookComment>();
            try {
                dbConnection.OpenConnection();
                // Query 'comments' table, where 'book_id'
                string query = "SELECT id, user_id, comment_datetime, text FROM comments WHERE book_id = @bookId ORDER BY comment_datetime DESC";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@bookId", bookId); // Renamed parameter and column book_id
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            int userId = Convert.ToInt32(reader["user_id"]);
                            comments.Add(new BookComment { // Use BookComment
                                Id = Convert.ToInt32(reader["id"]),
                                UserId = userId,
                                Text = reader["text"].ToString(),
                                CommentDateTime = Convert.ToDateTime(reader["comment_datetime"])
                            });
                        }
                    }
                }
                foreach (var comment in comments) // Assuming GetUserLoginById is still valid for any user_id
                    comment.UserLogin = GetUserLoginById(comment.UserId);
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении комментариев: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении комментариев: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return comments;
        }
        public bool AddComment(int userId, int bookId, string text) { // Renamed webnovelId to bookId
            try {
                dbConnection.OpenConnection();
                // Insert into 'comments' table, use 'book_id'
                string query = "INSERT INTO comments (user_id, book_id, comment_datetime, text) VALUES (@userId, @bookId, @commentDateTime, @text)";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@bookId", bookId); // Renamed parameter and column
                    cmd.Parameters.AddWithValue("@commentDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@text", text);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при добавлении комментария: " + ex.Message);
                return false;
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при добавлении комментария: " + ex.Message);
                return false;
            } finally {
                dbConnection.CloseConnection();
            }
        }
        // Renamed from AddNovelFile
        public bool AddBookFile(int bookId, string originalFileName, string destinationFilePath) {
            try {
                dbConnection.OpenConnection();
                // Insert into 'book_files' table, use 'book_id'
                // FilePath check is now for .fb2 (handled by DB constraint)
                string query = "INSERT INTO book_files (book_id, file_name, publication_date, file_path) VALUES (@bookId, @fileName, @publicationDate, @filePath)";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@bookId", bookId); // Renamed parameter and column
                    cmd.Parameters.AddWithValue("@fileName", Path.GetFileNameWithoutExtension(originalFileName)); // originalFileName should be .fb2
                    cmd.Parameters.AddWithValue("@publicationDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@filePath", destinationFilePath); // This path is stored in DB
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при добавлении файла книги: " + ex.Message); // Updated message
                return false;
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при добавлении файла книги: " + ex.Message); // Updated message
                return false;
            } finally {
                dbConnection.CloseConnection();
            }
        }
        // Renamed from DeleteNovel
        public bool DeleteBook(int bookId) {
            SqlTransaction transaction = null;
            try {
                dbConnection.OpenConnection();
                transaction = dbConnection.GetConnection().BeginTransaction();

                // Note: Physical file deletion logic for cover and book files should be reviewed.
                // The old code selected paths but didn't show explicit deletion of these physical files.
                // Assuming the current logic for DB record deletion is what needs to be adapted.

                // Delete from 'book_genres'
                string deleteGenresQuery = "DELETE FROM book_genres WHERE book_id = @bookId";
                using (SqlCommand cmdDeleteGenres = new SqlCommand(deleteGenresQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteGenres.Parameters.AddWithValue("@bookId", bookId);
                    cmdDeleteGenres.ExecuteNonQuery();
                }
                // Delete from 'comments'
                string deleteCommentsQuery = "DELETE FROM comments WHERE book_id = @bookId";
                using (SqlCommand cmdDeleteComments = new SqlCommand(deleteCommentsQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteComments.Parameters.AddWithValue("@bookId", bookId);
                    cmdDeleteComments.ExecuteNonQuery();
                }
                // Delete from 'book_files'
                string deleteFilesQuery = "DELETE FROM book_files WHERE book_id = @bookId";
                using (SqlCommand cmdDeleteFiles = new SqlCommand(deleteFilesQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteFiles.Parameters.AddWithValue("@bookId", bookId);
                    cmdDeleteFiles.ExecuteNonQuery();
                }
                // Delete from 'user_books' (new table, important for referential integrity)
                string deleteUserBooksQuery = "DELETE FROM user_books WHERE book_id = @bookId";
                using (SqlCommand cmdDeleteUserBooks = new SqlCommand(deleteUserBooksQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteUserBooks.Parameters.AddWithValue("@bookId", bookId);
                    cmdDeleteUserBooks.ExecuteNonQuery();
                }
                // Delete from 'books'
                string deleteBookQuery = "DELETE FROM books WHERE id = @bookId";
                using (SqlCommand cmdDeleteBook = new SqlCommand(deleteBookQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteBook.Parameters.AddWithValue("@bookId", bookId);
                    cmdDeleteBook.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при удалении книги: " + ex.Message); // Updated message
                if (transaction != null)
                    transaction.Rollback();
                return false;
            } catch (IOException ioEx) {
                Console.WriteLine("Ошибка файловой системы при удалении файлов книги: " + ioEx.Message); // Updated message
                if (transaction != null && transaction.Connection != null)
                    transaction.Rollback();
                return false;
            }
            catch (Exception ex) {
                Console.WriteLine("Общая ошибка при удалении книги: " + ex.Message); // Updated message
                if (transaction != null)
                    transaction.Rollback();
                return false;
            } finally {
                dbConnection.CloseConnection();
            }
        }
        // Renamed from CreateNovel
        public int CreateBook(string title, int adminId, string writerName, short? publicationYear, short? ageRating, string description, string coverPath, List<int> genreIds) {
            int newBookId = 0;
            SqlTransaction transaction = null;
            try {
                dbConnection.OpenConnection();
                transaction = dbConnection.GetConnection().BeginTransaction();
                string query = @"
                    INSERT INTO books (title, admin_id, writer_name, publication_year, age_rating, description, cover_path)
                    OUTPUT INSERTED.ID
                    VALUES (@title, @adminId, @writerName, @publicationYear, @ageRating, @description, @coverPath)"; // Table books, admin_id, writer_name
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection(), transaction)) {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@adminId", adminId); // Renamed from @writerId
                    cmd.Parameters.AddWithValue("@writerName", writerName); // New parameter
                    cmd.Parameters.AddWithValue("@publicationYear", (object)publicationYear ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ageRating", (object)ageRating ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@description", (object)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@coverPath", (object)coverPath ?? DBNull.Value);

                    newBookId = (int)cmd.ExecuteScalar();
                }
                if (newBookId > 0 && genreIds != null && genreIds.Count > 0) {
                    // Insert into 'book_genres'
                    string genreQuery = "INSERT INTO book_genres (book_id, genre_id) VALUES (@bookId, @genreId)";
                    foreach (int genreId in genreIds) {
                        using (SqlCommand genreCmd = new SqlCommand(genreQuery, dbConnection.GetConnection(), transaction)) {
                            genreCmd.Parameters.AddWithValue("@bookId", newBookId); // Renamed parameter
                            genreCmd.Parameters.AddWithValue("@genreId", genreId);
                            genreCmd.ExecuteNonQuery();
                        }
                    }
                }
                transaction.Commit();
                return newBookId;
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при создании книги: " + ex.Message); // Updated message
                if (transaction != null)
                    transaction.Rollback();
                return 0;
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при создании книги: " + ex.Message); // Updated message
                if (transaction != null)
                    transaction.Rollback();
                return 0;
            } finally {
                dbConnection.CloseConnection();
            }
        }
        public List<KeyValuePair<int, string>> GetAllGenres() {
            List<KeyValuePair<int, string>> allGenres = new List<KeyValuePair<int, string>>();
            try {
                dbConnection.OpenConnection();
                string query = "SELECT id, genre_name FROM genres ORDER BY genre_name ASC";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                           allGenres.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id"]), reader["genre_name"].ToString()));
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении всех жанров: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении всех жанров: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return allGenres;
        }
        public void Dispose() {
            if (dbConnection != null) {
                dbConnection.Dispose();
                dbConnection = null;
            }
        }
    }
}
