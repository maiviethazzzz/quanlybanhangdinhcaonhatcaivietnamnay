namespace QuanLyBanHang.MODEL
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        
        // Navigation
        public string CategoryName { get; set; }
    }
}
