namespace QuanLyBanHang.MODEL
{
    public class InvoiceDetail
    {
        public int DetailId { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        
        // Navigation
        public string ProductName { get; set; }
    }
}
