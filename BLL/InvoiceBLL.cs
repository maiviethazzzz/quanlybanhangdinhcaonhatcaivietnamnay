using System;
using System.Collections.Generic;
using QuanLyBanHang.DAL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.BLL
{
    public class InvoiceBLL
    {
        private InvoiceDAL invoiceDAL = new InvoiceDAL();
        
        // Tính tổng tiền giỏ hàng
        public decimal CalculateTotal(List<InvoiceDetail> details)
        {
            decimal total = 0;
            foreach (var item in details)
            {
                item.LineTotal = item.Quantity * item.UnitPrice;
                total += item.LineTotal;
            }
            return total;
        }
        
        // Tạo hóa đơn
        public int CreateInvoice(Invoice invoice)
        {
            if (invoice.Details.Count == 0)
                throw new Exception("Hóa đơn không có sản phẩm!");
                
            invoice.InvoiceDate = DateTime.Now;
            invoice.TotalAmount = CalculateTotal(invoice.Details);
            invoice.FinalAmount = invoice.TotalAmount; // Có thể có giảm giá
            
            return invoiceDAL.Insert(invoice);
        }
        
        public List<Invoice> GetAll()
        {
            return invoiceDAL.GetAll();
        }
    }
}
