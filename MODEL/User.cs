namespace QuanLyBanHang.MODEL
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        
        // Navigation
        public string RoleName { get; set; }
    }
}
