using System.Windows;

namespace QuanLyBanHang.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Hiển thị thông tin user
            if (LoginWindow.CurrentUser != null)
            {
                lblUser.Text = $"Xin chào: {LoginWindow.CurrentUser.FullName} ({LoginWindow.CurrentUser.RoleName})";
                
                // Phân quyền: Ẩn nút theo vai trò
                // Admin (RoleId=1): Toàn quyền
                // Manager (RoleId=2): Quản lý sản phẩm, người dùng, báo cáo
                // Staff (RoleId=3): Chỉ bán hàng, xem báo cáo cá nhân
                
                int roleId = LoginWindow.CurrentUser.RoleId;
                
                if (roleId == 3) // Staff
                {
                    btnUsers.Visibility = Visibility.Collapsed;
                    btnImport.Visibility = Visibility.Collapsed;
                }
            }
            
            // Mở màn hình POS mặc định
            mainFrame.Navigate(new POSPage());
        }
        
        private void btnPOS_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new POSPage());
            lblStatus.Text = "Bán hàng";
        }
        
        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new ProductPage());
            lblStatus.Text = "Quản lý sản phẩm";
        }
        
        private void btnCategories_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new CategoryPage());
            lblStatus.Text = "Quản lý danh mục";
        }
        
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new StockImportPage());
            lblStatus.Text = "Nhập hàng";
        }
        
        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            // Chỉ Admin mới được quản lý người dùng
            if (LoginWindow.CurrentUser?.RoleId != 1)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Phân quyền");
                return;
            }
            mainFrame.Navigate(new UserPage());
            lblStatus.Text = "Quản lý người dùng";
        }
        
        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new ReportPage());
            lblStatus.Text = "Báo cáo thống kê";
        }
        
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow.CurrentUser = null;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
