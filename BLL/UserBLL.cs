using QuanLyBanHang.DAL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.BLL
{
    public class UserBLL
    {
        private UserDAL userDAL;
        
        public UserBLL()
        {
            userDAL = new UserDAL();
        }
        
        // Đăng nhập - kiểm tra username/password
        public User Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            
            if (userDAL == null)
            {
                userDAL = new UserDAL();
            }
            
            return userDAL.Login(username, password);
        }
    }
}
