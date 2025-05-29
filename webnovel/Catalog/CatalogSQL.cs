using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace bookservice {
    public class CatalogSQL : IDisposable {
        private Connection dbConnection;
        private User user;
        private string searchTerm = "";
        private List<string> selectedGenres = new List<string>();
        private List<int> selectedAgeRatings = new List<int>();
        private string sortByYearDirection = "";
        public CatalogSQL() {
            dbConnection = new Connection();
            this.user = new User();
            updateUser(user);
        }
        public CatalogSQL(User user) {
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
        public void SetSearchTerm(string term) {
            searchTerm = term?.Trim() ?? "";
        }
        public void SetSelectedGenres(List<string> genres) {
            selectedGenres = genres ?? new List<string>();
        }
        public void SetSelectedAgeRatings(List<int> ageRatings) {
            selectedAgeRatings = ageRatings ?? new List<int>();
        }
        public void SetSortByYearDirection(string direction) {
            direction = direction?.ToUpper();
            if (direction == "ASC" || direction == "DESC")
                sortByYearDirection = direction;
            else
                sortByYearDirection = "";
        }
        public DataTable GetWebnovels() {
            DataTable dt = new DataTable();
            try {
                dbConnection.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT DISTINCT w.id, w.cover_path, w.title, w.publication_year ");
                queryBuilder.Append("FROM webnovels w ");
                if (selectedGenres.Any()) {
                    queryBuilder.Append("JOIN webnovel_genres wg ON w.id = wg.webnovel_id ");
                    queryBuilder.Append("JOIN genres g ON wg.genre_id = g.id ");
                }
                queryBuilder.Append("WHERE 1=1 ");
                if (!string.IsNullOrEmpty(searchTerm))
                    queryBuilder.Append("AND w.title LIKE @SearchTerm ");
                if (selectedGenres.Any()) {
                    List<string> genreParams = new List<string>();
                    for (int i = 0; i < selectedGenres.Count; i++)
                        genreParams.Add($"@Genre{i}");
                    queryBuilder.Append($"AND g.genre_name IN ({string.Join(", ", genreParams)}) ");
                }
                if (selectedAgeRatings.Any()) {
                    List<string> ageRatingParams = new List<string>();
                    for (int i = 0; i < selectedAgeRatings.Count; i++)
                        ageRatingParams.Add($"@AgeRating{i}");
                    queryBuilder.Append($"AND w.age_rating IN ({string.Join(", ", ageRatingParams)}) ");
                }
                if (!string.IsNullOrEmpty(sortByYearDirection))
                    queryBuilder.Append($"ORDER BY w.publication_year {sortByYearDirection}, w.title ASC ");
                else
                    queryBuilder.Append("ORDER BY w.id ASC ");
                using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), dbConnection.GetConnection())) {
                    if (!string.IsNullOrEmpty(searchTerm))
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    if (selectedGenres.Any())
                        for (int i = 0; i < selectedGenres.Count; i++)
                            cmd.Parameters.AddWithValue($"@Genre{i}", selectedGenres[i]);

                    if (selectedAgeRatings.Any())
                        for (int i = 0; i < selectedAgeRatings.Count; i++)
                            cmd.Parameters.AddWithValue($"@AgeRating{i}", selectedAgeRatings[i]);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении веб-романов: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении веб-романов: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return dt;
        }
        public List<string> GetWebnovelTitles() {
            List<string> titles = new List<string>();
            try {
                dbConnection.OpenConnection();
                string query = "SELECT title FROM webnovels ORDER BY title ASC";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                            titles.Add(reader["title"].ToString());
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении названий веб-романов: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении названий веб-романов: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return titles;
        }
        public List<string> GetGenres() {
            List<string> genres = new List<string>();
            try {
                dbConnection.OpenConnection();
                string query = "SELECT genre_name FROM genres ORDER BY genre_name ASC";
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                            genres.Add(reader["genre_name"].ToString());
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении жанров: " + ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении жанров: " + ex.Message);
            } finally {
                dbConnection.CloseConnection();
            }
            return genres;
        }
        public void Dispose() {
            if (dbConnection != null) {
                dbConnection.Dispose();
                dbConnection = null;
            }
        }
        ~CatalogSQL() {
            Dispose();
        }
    }
}
