using System;
using System.Data.SqlClient;

namespace bookservice {
    public class Connection {
        private string connectionString;
        private SqlConnection con;
        public Connection() {
            con = new SqlConnection(connectionString);
        }
        public void SetСonnectionString(string connectionString) {
            this.connectionString = connectionString;
            con = new SqlConnection(this.connectionString);
        }
        public void OpenConnection() {
            try {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка открытия соединения: " + ex.Message);
                throw;
            }
        }
        public void CloseConnection() {
            try {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            } catch (SqlException ex) {
                Console.WriteLine("Ошибка закрытия соединения: " + ex.Message);
            }
        }
        public SqlConnection GetConnection() {
            return con;
        }
        public void Dispose() {
            if (con != null) {
                con.Dispose();
                con = null;
            }
        }
    }
}
