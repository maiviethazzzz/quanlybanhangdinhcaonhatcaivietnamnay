using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.DAL
{
    public class InvoiceDAL
    {
        // Tạo hóa đơn mới
        public int Insert(Invoice invoice)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // Thêm hóa đơn
                    string sql = @"INSERT INTO Invoices (InvoiceDate, UserId, CustomerName, TotalAmount, FinalAmount) 
                                   VALUES (@InvoiceDate, @UserId, @CustomerName, @TotalAmount, @FinalAmount);
                                   SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                    cmd.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                    cmd.Parameters.AddWithValue("@UserId", invoice.UserId);
                    cmd.Parameters.AddWithValue("@CustomerName", (object)invoice.CustomerName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
                    cmd.Parameters.AddWithValue("@FinalAmount", invoice.FinalAmount);
                    
                    int invoiceId = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    // Thêm chi tiết hóa đơn
                    foreach (var detail in invoice.Details)
                    {
                        string sqlDetail = @"INSERT INTO InvoiceDetails (InvoiceId, ProductId, Quantity, UnitPrice, LineTotal) 
                                             VALUES (@InvoiceId, @ProductId, @Quantity, @UnitPrice, @LineTotal)";
                        SqlCommand cmdDetail = new SqlCommand(sqlDetail, conn, transaction);
                        cmdDetail.Parameters.AddWithValue("@InvoiceId", invoiceId);
                        cmdDetail.Parameters.AddWithValue("@ProductId", detail.ProductId);
                        cmdDetail.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        cmdDetail.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                        cmdDetail.Parameters.AddWithValue("@LineTotal", detail.LineTotal);
                        cmdDetail.ExecuteNonQuery();
                        
                        // Trừ tồn kho
                        string sqlUpdateQty = "UPDATE Products SET Quantity = Quantity - @Qty WHERE ProductId = @ProductId";
                        SqlCommand cmdUpdateQty = new SqlCommand(sqlUpdateQty, conn, transaction);
                        cmdUpdateQty.Parameters.AddWithValue("@Qty", detail.Quantity);
                        cmdUpdateQty.Parameters.AddWithValue("@ProductId", detail.ProductId);
                        cmdUpdateQty.ExecuteNonQuery();
                    }
                    
                    transaction.Commit();
                    return invoiceId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
        // Lấy danh sách hóa đơn
        public List<Invoice> GetAll()
        {
            List<Invoice> list = new List<Invoice>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT i.*, u.FullName as CashierName 
                               FROM Invoices i 
                               INNER JOIN Users u ON i.UserId = u.UserId 
                               ORDER BY i.InvoiceDate DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Invoice
                    {
                        InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                        InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        CustomerName = reader["CustomerName"]?.ToString(),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        FinalAmount = Convert.ToDecimal(reader["FinalAmount"]),
                        CashierName = reader["CashierName"].ToString()
                    });
                }
            }
            return list;
        }
        
        // Báo cáo doanh số theo nhân viên
        public List<SalesReport> GetSalesByStaff(DateTime fromDate, DateTime toDate)
        {
            List<SalesReport> list = new List<SalesReport>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT u.FullName as StaffName, COUNT(i.InvoiceId) as TotalInvoices, 
                                      SUM(i.FinalAmount) as TotalSales
                               FROM Invoices i 
                               INNER JOIN Users u ON i.UserId = u.UserId
                               WHERE i.InvoiceDate BETWEEN @FromDate AND @ToDate
                               GROUP BY u.UserId, u.FullName
                               ORDER BY TotalSales DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new SalesReport
                    {
                        StaffName = reader["StaffName"].ToString(),
                        TotalInvoices = Convert.ToInt32(reader["TotalInvoices"]),
                        TotalSales = Convert.ToDecimal(reader["TotalSales"])
                    });
                }
            }
            return list;
        }
        
        // Báo cáo doanh số tổng thể theo ngày
        public List<DailySalesReport> GetDailySales(DateTime fromDate, DateTime toDate)
        {
            List<DailySalesReport> list = new List<DailySalesReport>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"SELECT CAST(InvoiceDate as DATE) as SaleDate, 
                                      COUNT(InvoiceId) as TotalInvoices, 
                                      SUM(FinalAmount) as TotalSales
                               FROM Invoices 
                               WHERE InvoiceDate BETWEEN @FromDate AND @ToDate
                               GROUP BY CAST(InvoiceDate as DATE)
                               ORDER BY SaleDate DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DailySalesReport
                    {
                        SaleDate = Convert.ToDateTime(reader["SaleDate"]),
                        TotalInvoices = Convert.ToInt32(reader["TotalInvoices"]),
                        TotalSales = Convert.ToDecimal(reader["TotalSales"])
                    });
                }
            }
            return list;
        }
    }
    
    // Model cho báo cáo
    public class SalesReport
    {
        public string StaffName { get; set; }
        public int TotalInvoices { get; set; }
        public decimal TotalSales { get; set; }
    }
    
    public class DailySalesReport
    {
        public DateTime SaleDate { get; set; }
        public int TotalInvoices { get; set; }
        public decimal TotalSales { get; set; }
    }
}
