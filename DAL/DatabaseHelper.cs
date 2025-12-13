using System.Data.SqlClient;

namespace QuanLyBanHang.DAL
{
    public static class DatabaseHelper
    {
        // Chuỗi kết nối đến SQL Server
        // Thêm TrustServerCertificate=True để tránh lỗi SSL
        private static string connectionString = @"Server=WINDOWS-10\SQLEXPRESS;Database=QuanLyBanHang;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        
        public static void SetConnectionString(string connStr)
        {
            connectionString = connStr;
        }
        
        // Test kết nối
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
