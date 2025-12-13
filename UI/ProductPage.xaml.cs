using System;
using System.Windows;
using System.Windows.Controls;
using QuanLyBanHang.BLL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.UI
{
    public partial class ProductPage : Page
    {
        private ProductBLL productBLL = new ProductBLL();
        private CategoryBLL categoryBLL = new CategoryBLL();
        private Product selectedProduct = null;
        
        public ProductPage()
        {
            InitializeComponent();
            LoadData();
        }
        
        private void LoadData()
        {
            dgProducts.ItemsSource = productBLL.GetAll();
            cmbCategory.ItemsSource = categoryBLL.GetAll();
        }
        
        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product product)
            {
                selectedProduct = product;
                txtName.Text = product.ProductName;
                cmbCategory.SelectedValue = product.CategoryId;
                txtCostPrice.Text = product.CostPrice.ToString();
                txtSellingPrice.Text = product.SellingPrice.ToString();
                txtQuantity.Text = product.Quantity.ToString();
            }
        }
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new Product
                {
                    ProductName = txtName.Text.Trim(),
                    CategoryId = (int)(cmbCategory.SelectedValue ?? 0),
                    CostPrice = decimal.Parse(txtCostPrice.Text),
                    SellingPrice = decimal.Parse(txtSellingPrice.Text),
                    Quantity = int.Parse(txtQuantity.Text)
                };
                
                productBLL.Insert(product);
                MessageBox.Show("Thêm thành công!");
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Chọn sản phẩm cần sửa!");
                return;
            }
            
            try
            {
                selectedProduct.ProductName = txtName.Text.Trim();
                selectedProduct.CategoryId = (int)(cmbCategory.SelectedValue ?? 0);
                selectedProduct.CostPrice = decimal.Parse(txtCostPrice.Text);
                selectedProduct.SellingPrice = decimal.Parse(txtSellingPrice.Text);
                selectedProduct.Quantity = int.Parse(txtQuantity.Text);
                
                productBLL.Update(selectedProduct);
                MessageBox.Show("Cập nhật thành công!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Chọn sản phẩm cần xóa!");
                return;
            }
            
            if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    productBLL.Delete(selectedProduct.ProductId);
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        
        private void ClearForm()
        {
            selectedProduct = null;
            txtName.Text = "";
            cmbCategory.SelectedIndex = -1;
            txtCostPrice.Text = "";
            txtSellingPrice.Text = "";
            txtQuantity.Text = "";
        }
    }
}
