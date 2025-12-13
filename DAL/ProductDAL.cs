using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.DAL
{
    public class ProductDAL
    {
        // Lấy tất cả sản phẩm
        public List<Product> GetAll()
        {
            List<Product> list = new List<Product>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT p.*, c.CategoryName 
                               FROM Products p 
                               LEFT JOIN Categories c ON p.CategoryId = c.CategoryId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        CostPrice = Convert.ToDecimal(reader["CostPrice"]),
                        SellingPrice = Convert.ToDecimal(reader["SellingPrice"]),
                        CategoryName = reader["CategoryName"]?.ToString()
                    });
                }
            }
            return list;
        }
        
        // Tìm kiếm sản phẩm
        public List<Product> Search(string keyword)
        {
            List<Product> list = new List<Product>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT p.*, c.CategoryName 
                               FROM Products p 
                               LEFT JOIN Categories c ON p.CategoryId = c.CategoryId
                               WHERE p.ProductName LIKE @Keyword";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        CostPrice = Convert.ToDecimal(reader["CostPrice"]),
                        SellingPrice = Convert.ToDecimal(reader["SellingPrice"]),
                        CategoryName = reader["CategoryName"]?.ToString()
                    });
                }
            }
            return list;
        }
        
        // Thêm sản phẩm
        public int Insert(Product product)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"INSERT INTO Products (ProductName, CategoryId, Quantity, CostPrice, SellingPrice) 
                               VALUES (@ProductName, @CategoryId, @Quantity, @CostPrice, @SellingPrice);
                               SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@CostPrice", product.CostPrice);
                cmd.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        
        // Cập nhật sản phẩm
        public void Update(Product product)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"UPDATE Products 
                               SET ProductName = @ProductName, CategoryId = @CategoryId, 
                                   Quantity = @Quantity, CostPrice = @CostPrice, SellingPrice = @SellingPrice 
                               WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@CostPrice", product.CostPrice);
                cmd.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        // Xóa sản phẩm
        public void Delete(int productId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM Products WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        // Trừ tồn kho khi bán
        public void UpdateQuantity(int productId, int soldQuantity)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE Products SET Quantity = Quantity - @SoldQty WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@SoldQty", soldQuantity);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
