using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QuanLyBanHang.BLL;
using QuanLyBanHang.DAL;

namespace QuanLyBanHang.UI
{
    public partial class ReportPage : Page
    {
        private InvoiceBLL invoiceBLL = new InvoiceBLL();
        private InvoiceDAL invoiceDAL = new InvoiceDAL();
        private ProductBLL productBLL = new ProductBLL();
        
        public ReportPage()
        {
            InitializeComponent();
            
            // Mặc định: 30 ngày gần nhất
            dpFrom.SelectedDate = DateTime.Today.AddDays(-30);
            dpTo.SelectedDate = DateTime.Today;
        }
        
        private void btnInvoices_Click(object sender, RoutedEventArgs e)
        {
            var invoices = invoiceBLL.GetAll();
            dgReport.ItemsSource = invoices;
            
            decimal total = invoices.Sum(x => x.FinalAmount);
            lblSummary.Text = $"Tổng số hóa đơn: {invoices.Count} | Tổng doanh thu: {total:N0} VND";
        }
        
        private void btnSalesByStaff_Click(object sender, RoutedEventArgs e)
        {
            DateTime fromDate = dpFrom.SelectedDate ?? DateTime.Today.AddDays(-30);
            DateTime toDate = dpTo.SelectedDate ?? DateTime.Today;
            toDate = toDate.AddDays(1); // Để bao gồm cả ngày cuối
            
            var data = invoiceDAL.GetSalesByStaff(fromDate, toDate);
            dgReport.ItemsSource = data;
            
            decimal total = data.Sum(x => x.TotalSales);
            lblSummary.Text = $"Tổng doanh thu: {total:N0} VND";
        }
        
        private void btnDailySales_Click(object sender, RoutedEventArgs e)
        {
            DateTime fromDate = dpFrom.SelectedDate ?? DateTime.Today.AddDays(-30);
            DateTime toDate = dpTo.SelectedDate ?? DateTime.Today;
            toDate = toDate.AddDays(1);
            
            var data = invoiceDAL.GetDailySales(fromDate, toDate);
            dgReport.ItemsSource = data;
            
            decimal total = data.Sum(x => x.TotalSales);
            lblSummary.Text = $"Tổng doanh thu: {total:N0} VND";
        }
        
        private void btnLowStock_Click(object sender, RoutedEventArgs e)
        {
            var lowStock = productBLL.GetLowStock(10);
            dgReport.ItemsSource = lowStock;
            lblSummary.Text = $"Có {lowStock.Count} sản phẩm cần nhập thêm (dưới 10)";
        }
    }
}
