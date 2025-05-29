using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace bookservice {
    public class NovelDetails {
        public int Id { get; set; }
        public string Title { get; set; }
        public int WriterId { get; set; }
        public string WriterLogin { get; set; }
        public short? PublicationYear { get; set; }
        public short? AgeRating { get; set; }
        public string Description { get; set; }
        public string CoverPath { get; set; }
    }
    public class NovelFile {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
    public class NovelComment {
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
        public NovelDetails GetNovelDetails(int novelId) {
            NovelDetails details = null;
            try {
                dbConnection.OpenConnection();
                string query = "SELECT id, title, writer_id, publication_year, age_rating, description, cover_path FROM webnovels WHERE id = @novelId";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@novelId", novelId);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            details = new NovelDetails {
                                Id = Convert.ToInt32(reader["id"]),
                                Title = reader["title"].ToString(),
                                WriterId = Convert.ToInt32(reader["writer_id"]),
                                PublicationYear = reader["publication_year"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["publication_year"]),
                                AgeRating = reader["age_rating"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["age_rating"]),
                                Description = reader["description"].ToString(),
                                CoverPath = reader["cover_path"].ToString()
                            };
                        }
                    }
                }
                if (details != null)
                    details.WriterLogin = GetUserLoginById(details.WriterId);
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении деталей веб-романа: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении деталей веб-романа: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return details;
        }
        public List<string> GetNovelGenres(int novelId) {
            List<string> genres = new List<string>();
            try {
                dbConnection.OpenConnection();
                string query = @"
                    SELECT g.genre_name
                    FROM genres g
                    INNER JOIN webnovel_genres wg ON g.id = wg.genre_id
                    WHERE wg.webnovel_id = @novelId
                    ORDER BY g.genre_name ASC";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@novelId", novelId);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                            genres.Add(reader["genre_name"].ToString());
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении жанров веб-романа: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении жанров веб-романа: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return genres;
        }
        public List<NovelFile> GetNovelFiles(int novelId) {
            List<NovelFile> files = new List<NovelFile>();
            try {
                dbConnection.OpenConnection();
                string query = "SELECT id, file_name, file_path, publication_date FROM novel_files WHERE webnovel_id = @novelId ORDER BY file_name ASC";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@novelId", novelId);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            files.Add(new NovelFile {
                                Id = Convert.ToInt32(reader["id"]),
                                FileName = reader["file_name"].ToString(),
                                FilePath = reader["file_path"].ToString(),
                                PublicationDate = reader["publication_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["publication_date"])
                            });
                        }
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении файлов веб-романа: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении файлов веб-романа: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return files;
        }
        public List<NovelComment> GetNovelComments(int novelId) {
            List<NovelComment> comments = new List<NovelComment>();
            try {
                dbConnection.OpenConnection();
                string query = "SELECT id, user_id, comment_datetime, text FROM comments WHERE webnovel_id = @novelId ORDER BY comment_datetime DESC";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@novelId", novelId);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            int userId = Convert.ToInt32(reader["user_id"]);
                            comments.Add(new NovelComment {
                                Id = Convert.ToInt32(reader["id"]),
                                UserId = userId,
                                Text = reader["text"].ToString(),
                                CommentDateTime = Convert.ToDateTime(reader["comment_datetime"])
                            });
                        }
                    }
                }
                foreach (var comment in comments)
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
        public bool AddComment(int userId, int webnovelId, string text) {
            try {
                dbConnection.OpenConnection();
                string query = "INSERT INTO comments (user_id, webnovel_id, comment_datetime, text) VALUES (@userId, @webnovelId, @commentDateTime, @text)";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@webnovelId", webnovelId);
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
        public bool AddNovelFile(int webnovelId, string originalFileName, string destinationFilePath) {
            try {
                dbConnection.OpenConnection();
                string query = "INSERT INTO novel_files (webnovel_id, file_name, publication_date, file_path) VALUES (@webnovelId, @fileName, @publicationDate, @filePath)";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    cmd.Parameters.AddWithValue("@webnovelId", webnovelId);
                    cmd.Parameters.AddWithValue("@fileName", Path.GetFileNameWithoutExtension(originalFileName));
                    cmd.Parameters.AddWithValue("@publicationDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@filePath", destinationFilePath);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при добавлении файла веб-романа: " + ex.Message);
                return false;
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при добавлении файла веб-романа: " + ex.Message);
                return false;
            } finally {
                dbConnection.CloseConnection();
            }
        }
        public bool DeleteNovel(int novelId) {
            SqlTransaction transaction = null;
            try {
                dbConnection.OpenConnection();
                transaction = dbConnection.GetConnection().BeginTransaction();

                string coverPathToDelete = null;
                List<string> filesToDelete = new List<string>();

                string getCoverQuery = "SELECT cover_path FROM webnovels WHERE id = @novelId";
                using (SqlCommand cmdGetCover = new SqlCommand(getCoverQuery, dbConnection.GetConnection(), transaction)) {
                    cmdGetCover.Parameters.AddWithValue("@novelId", novelId);
                    object result = cmdGetCover.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        coverPathToDelete = result.ToString();
                }

                string getFilesQuery = "SELECT file_path FROM novel_files WHERE webnovel_id = @novelId";
                using (SqlCommand cmdGetFiles = new SqlCommand(getFilesQuery, dbConnection.GetConnection(), transaction)) {
                    cmdGetFiles.Parameters.AddWithValue("@novelId", novelId);
                    using (SqlDataReader reader = cmdGetFiles.ExecuteReader()) {
                        while (reader.Read())
                            filesToDelete.Add(reader["file_path"].ToString());
                    }
                }
                string deleteGenresQuery = "DELETE FROM webnovel_genres WHERE webnovel_id = @novelId";
                using (SqlCommand cmdDeleteGenres = new SqlCommand(deleteGenresQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteGenres.Parameters.AddWithValue("@novelId", novelId);
                    cmdDeleteGenres.ExecuteNonQuery();
                }
                string deleteCommentsQuery = "DELETE FROM comments WHERE webnovel_id = @novelId";
                using (SqlCommand cmdDeleteComments = new SqlCommand(deleteCommentsQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteComments.Parameters.AddWithValue("@novelId", novelId);
                    cmdDeleteComments.ExecuteNonQuery();
                }
                string deleteFilesQuery = "DELETE FROM novel_files WHERE webnovel_id = @novelId";
                using (SqlCommand cmdDeleteFiles = new SqlCommand(deleteFilesQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteFiles.Parameters.AddWithValue("@novelId", novelId);
                    cmdDeleteFiles.ExecuteNonQuery();
                }
                string deleteNovelQuery = "DELETE FROM webnovels WHERE id = @novelId";
                using (SqlCommand cmdDeleteNovel = new SqlCommand(deleteNovelQuery, dbConnection.GetConnection(), transaction)) {
                    cmdDeleteNovel.Parameters.AddWithValue("@novelId", novelId);
                    cmdDeleteNovel.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при удалении веб-романа: " + ex.Message);
                if (transaction != null)
                    transaction.Rollback();
                return false;
            } catch (IOException ioEx) {
                Console.WriteLine("Ошибка файловой системы при удалении файлов веб-романа: " + ioEx.Message);
                if (transaction != null && transaction.Connection != null)
                    transaction.Rollback();
                return false;
            }
            catch (Exception ex) {
                Console.WriteLine("Общая ошибка при удалении веб-романа: " + ex.Message);
                if (transaction != null)
                    transaction.Rollback();
                return false;
            } finally {
                dbConnection.CloseConnection();
            }
        }
        public int CreateNovel(string title, int writerId, short? publicationYear, short? ageRating, string description, string coverPath, List<int> genreIds) {
            int newNovelId = 0;
            SqlTransaction transaction = null;
            try {
                dbConnection.OpenConnection();
                transaction = dbConnection.GetConnection().BeginTransaction();
                string query = @"
                    INSERT INTO webnovels (title, writer_id, publication_year, age_rating, description, cover_path)
                    OUTPUT INSERTED.ID
                    VALUES (@title, @writerId, @publicationYear, @ageRating, @description, @coverPath)";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection(), transaction)) {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@writerId", writerId);
                    cmd.Parameters.AddWithValue("@publicationYear", (object)publicationYear ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ageRating", (object)ageRating ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@description", (object)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@coverPath", (object)coverPath ?? DBNull.Value);

                    newNovelId = (int)cmd.ExecuteScalar();
                }
                if (newNovelId > 0 && genreIds != null && genreIds.Count > 0) {
                    string genreQuery = "INSERT INTO webnovel_genres (webnovel_id, genre_id) VALUES (@webnovelId, @genreId)";
                    foreach (int genreId in genreIds) {
                        using (SqlCommand genreCmd = new SqlCommand(genreQuery, dbConnection.GetConnection(), transaction)) {
                            genreCmd.Parameters.AddWithValue("@webnovelId", newNovelId);
                            genreCmd.Parameters.AddWithValue("@genreId", genreId);
                            genreCmd.ExecuteNonQuery();
                        }
                    }
                }
                transaction.Commit();
                return newNovelId;
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при создании веб-романа: " + ex.Message);
                if (transaction != null)
                    transaction.Rollback();
                return 0;
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при создании веб-романа: " + ex.Message);
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
