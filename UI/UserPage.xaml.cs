using System;
using System.Windows;
using System.Windows.Controls;
using QuanLyBanHang.DAL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.UI
{
    public partial class UserPage : Page
    {
        private UserDAL userDAL = new UserDAL();
        private User selected = null;
        
        public UserPage()
        {
            InitializeComponent();
            LoadData();
        }
        
        private void LoadData()
        {
            dgUsers.ItemsSource = userDAL.GetAll();
            cmbRole.ItemsSource = userDAL.GetRoles();
        }
        
        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem is User user)
            {
                selected = user;
                txtUsername.Text = user.Username;
                txtFullName.Text = user.FullName;
                cmbRole.SelectedValue = user.RoleId;
                txtPassword.Password = "";
            }
        }
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            
            try
            {
                User user = new User
                {
                    Username = txtUsername.Text.Trim(),
                    PasswordHash = txtPassword.Password,
                    FullName = txtFullName.Text.Trim(),
                    RoleId = (int)(cmbRole.SelectedValue ?? 3)
                };
                userDAL.Insert(user);
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
            if (selected == null) { MessageBox.Show("Chọn người dùng!"); return; }
            
            try
            {
                selected.Username = txtUsername.Text.Trim();
                selected.FullName = txtFullName.Text.Trim();
                selected.RoleId = (int)(cmbRole.SelectedValue ?? 3);
                userDAL.Update(selected);
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
            if (selected == null) { MessageBox.Show("Chọn người dùng!"); return; }
            
            if (MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    userDAL.Delete(selected.UserId);
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
        
        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (selected == null) { MessageBox.Show("Chọn người dùng!"); return; }
            if (string.IsNullOrEmpty(txtPassword.Password)) { MessageBox.Show("Nhập mật khẩu mới!"); return; }
            
            try
            {
                userDAL.ChangePassword(selected.UserId, txtPassword.Password);
                MessageBox.Show("Đổi mật khẩu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        
        private void ClearForm()
        {
            selected = null;
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtFullName.Text = "";
            cmbRole.SelectedIndex = -1;
        }
    }
}
