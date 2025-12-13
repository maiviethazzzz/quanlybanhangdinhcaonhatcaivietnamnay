using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.DAL
{
    public class CategoryDAL
    {
        public List<Category> GetAll()
        {
            List<Category> list = new List<Category>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT * FROM Categories";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Category
                    {
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString()
                    });
                }
            }
            return list;
        }
        
        public int Insert(Category category)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        
        public void Update(Category category)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        public void Delete(int categoryId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM Categories WHERE CategoryId = @CategoryId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
