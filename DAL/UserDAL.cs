using System;
using System.Data.SqlClient;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.DAL
{
    public class UserDAL
    {
        // Đăng nhập - kiểm tra username và password
        public User Login(string username, string password)
        {
            User user = null;
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT u.UserId, u.Username, u.FullName, u.RoleId, r.RoleName 
                               FROM Users u 
                               INNER JOIN Roles r ON u.RoleId = r.RoleId 
                               WHERE u.Username = @Username AND u.PasswordHash = @Password";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader["UserId"]),
                            Username = reader.IsDBNull(1) ? "" : reader["Username"].ToString(),
                            FullName = reader.IsDBNull(2) ? "" : reader["FullName"].ToString(),
                            RoleId = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader.IsDBNull(4) ? "" : reader["RoleName"].ToString()
                        };
                    }
                }
            }
            return user;
        }
        
        // Lấy tất cả users
        public System.Collections.Generic.List<User> GetAll()
        {
            var list = new System.Collections.Generic.List<User>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT u.*, r.RoleName FROM Users u 
                               INNER JOIN Roles r ON u.RoleId = r.RoleId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            Username = reader["Username"]?.ToString() ?? "",
                            FullName = reader["FullName"]?.ToString() ?? "",
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"]?.ToString() ?? ""
                        });
                    }
                }
            }
            return list;
        }
        
        // Thêm user
        public int Insert(User user)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"INSERT INTO Users (Username, PasswordHash, FullName, RoleId) 
                               VALUES (@Username, @Password, @FullName, @RoleId);
                               SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", user.Username ?? "");
                cmd.Parameters.AddWithValue("@Password", user.PasswordHash ?? "");
                cmd.Parameters.AddWithValue("@FullName", user.FullName ?? "");
                cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        
        // Cập nhật user
        public void Update(User user)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"UPDATE Users SET Username=@Username, FullName=@FullName, RoleId=@RoleId 
                               WHERE UserId=@UserId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", user.UserId);
                cmd.Parameters.AddWithValue("@Username", user.Username ?? "");
                cmd.Parameters.AddWithValue("@FullName", user.FullName ?? "");
                cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        // Đổi mật khẩu
        public void ChangePassword(int userId, string newPassword)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE Users SET PasswordHash=@Password WHERE UserId=@UserId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Password", newPassword ?? "");
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        // Xóa user
        public void Delete(int userId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM Users WHERE UserId=@UserId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        // Lấy danh sách Roles
        public System.Collections.Generic.List<Role> GetRoles()
        {
            var list = new System.Collections.Generic.List<Role>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT * FROM Roles";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Role
                        {
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"]?.ToString() ?? ""
                        });
                    }
                }
            }
            return list;
        }
    }
}
