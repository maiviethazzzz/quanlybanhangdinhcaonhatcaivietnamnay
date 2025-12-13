using System;
using System.Collections.Generic;

namespace QuanLyBanHang.MODEL
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        
        // Navigation
        public string CashierName { get; set; }
        public List<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();
    }
}
