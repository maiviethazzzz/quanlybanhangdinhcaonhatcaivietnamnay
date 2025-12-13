using System.Windows;
using QuanLyBanHang.DAL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.UI
{
    public partial class LoginWindow : Window
    {
        public static User CurrentUser { get; set; }
        
        public LoginWindow()
        {
            InitializeComponent();
            txtUsername.Focus();
        }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text?.Trim() ?? "";
            string password = txtPassword.Password ?? "";
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Vui lòng nhập đầy đủ thông tin!";
                return;
            }
            
            try
            {
                // Gọi trực tiếp DAL thay vì qua BLL để tránh lỗi
                UserDAL userDAL = new UserDAL();
                User user = userDAL.Login(username, password);
                
                if (user != null)
                {
                    CurrentUser = user;
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    lblError.Text = "Sai tên đăng nhập hoặc mật khẩu!";
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                lblError.Text = "Lỗi CSDL: " + sqlEx.Message;
            }
            catch (System.Exception ex)
            {
                lblError.Text = "Lỗi: " + ex.Message + "\n" + ex.StackTrace;
            }
        }
        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
