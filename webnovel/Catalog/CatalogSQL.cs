﻿using System;
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
        public DataTable GetBooks() { // Renamed from GetWebnovels
            DataTable dt = new DataTable();
            try {
                dbConnection.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT DISTINCT b.id, b.cover_path, b.title, b.publication_year "); // Alias w to b for books
                queryBuilder.Append("FROM books b "); // Renamed webnovels to books
                if (selectedGenres.Any()) {
                    queryBuilder.Append("JOIN book_genres bg ON b.id = bg.book_id "); // Renamed webnovel_genres to book_genres, wg to bg, webnovel_id to book_id
                    queryBuilder.Append("JOIN genres g ON bg.genre_id = g.id ");
                }
                queryBuilder.Append("WHERE 1=1 ");
                if (!string.IsNullOrEmpty(searchTerm))
                    queryBuilder.Append("AND b.title LIKE @SearchTerm "); // Alias w to b
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
                    queryBuilder.Append($"AND b.age_rating IN ({string.Join(", ", ageRatingParams)}) "); // Alias w to b
                }
                if (!string.IsNullOrEmpty(sortByYearDirection))
                    queryBuilder.Append($"ORDER BY b.publication_year {sortByYearDirection}, b.title ASC "); // Alias w to b
                else
                    queryBuilder.Append("ORDER BY b.id ASC "); // Alias w to b
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
                Console.WriteLine("Ошибка SQL при получении книг: " + ex.Message); // Updated message
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении книг: " + ex.Message); // Updated message
            } finally {
                dbConnection.CloseConnection();
            }
            return dt;
        }
        public List<string> GetBookTitles() { // Renamed from GetWebnovelTitles
            List<string> titles = new List<string>();
            try {
                dbConnection.OpenConnection();
                string query = "SELECT title FROM books ORDER BY title ASC"; // Renamed webnovels to books
                using (SqlCommand cmd = new SqlCommand(query, dbConnection.GetConnection())) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                            titles.Add(reader["title"].ToString());
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка SQL при получении названий книг: " + ex.Message); // Updated message
            } catch (Exception ex) {
                Console.WriteLine("Общая ошибка при получении названий книг: " + ex.Message); // Updated message
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
