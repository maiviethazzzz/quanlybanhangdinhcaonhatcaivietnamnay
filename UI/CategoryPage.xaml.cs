using System;
using System.Windows;
using System.Windows.Controls;
using QuanLyBanHang.BLL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.UI
{
    public partial class CategoryPage : Page
    {
        private CategoryBLL categoryBLL = new CategoryBLL();
        private Category selected = null;
        
        public CategoryPage()
        {
            InitializeComponent();
            LoadData();
        }
        
        private void LoadData()
        {
            dgCategories.ItemsSource = categoryBLL.GetAll();
        }
        
        private void dgCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategories.SelectedItem is Category category)
            {
                selected = category;
                txtName.Text = category.CategoryName;
            }
        }
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                categoryBLL.Insert(new Category { CategoryName = txtName.Text.Trim() });
                MessageBox.Show("Thêm thành công!");
                LoadData();
                txtName.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selected == null) { MessageBox.Show("Chọn danh mục!"); return; }
            
            try
            {
                selected.CategoryName = txtName.Text.Trim();
                categoryBLL.Update(selected);
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
            if (selected == null) { MessageBox.Show("Chọn danh mục!"); return; }
            
            if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    categoryBLL.Delete(selected.CategoryId);
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    txtName.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
    }
}
