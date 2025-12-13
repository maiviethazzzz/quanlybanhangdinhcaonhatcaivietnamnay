using System;
using System.Windows;
using System.Windows.Controls;
using QuanLyBanHang.BLL;
using QuanLyBanHang.DAL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.UI
{
    public partial class StockImportPage : Page
    {
        private ProductBLL productBLL = new ProductBLL();
        private ProductDAL productDAL = new ProductDAL();
        private Product selected = null;
        
        public StockImportPage()
        {
            InitializeComponent();
            LoadData();
        }
        
        private void LoadData()
        {
            dgProducts.ItemsSource = productBLL.GetAll();
        }
        
        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product product)
            {
                selected = product;
                lblProduct.Text = product.ProductName;
                txtCostPrice.Text = product.CostPrice.ToString();
            }
        }
        
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Chọn sản phẩm cần nhập!");
                return;
            }
            
            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }
            
            try
            {
                // Cập nhật số lượng
                selected.Quantity += qty;
                
                // Cập nhật giá nhập nếu có
                if (decimal.TryParse(txtCostPrice.Text, out decimal newCost) && newCost > 0)
                {
                    selected.CostPrice = newCost;
                }
                
                productDAL.Update(selected);
                
                lblMessage.Text = $"Đã nhập {qty} {selected.ProductName}.\nTồn kho mới: {selected.Quantity}";
                LoadData();
                txtQuantity.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
