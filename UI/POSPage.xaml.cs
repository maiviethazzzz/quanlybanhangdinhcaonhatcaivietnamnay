using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyBanHang.BLL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.UI
{
    public partial class POSPage : Page
    {
        private ProductBLL productBLL = new ProductBLL();
        private InvoiceBLL invoiceBLL = new InvoiceBLL();
        private List<InvoiceDetail> cart = new List<InvoiceDetail>();
        
        private decimal subTotal = 0;
        private decimal discountPercent = 0;
        private decimal discountAmount = 0;
        private decimal vatAmount = 0;
        private decimal total = 0;
        private bool isLoaded = false;
        
        public POSPage()
        {
            InitializeComponent();
            this.Loaded += POSPage_Loaded;
        }
        
        private void POSPage_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            LoadProducts();
            UpdateCart();
        }
        
        private void LoadProducts()
        {
            try
            {
                dgProducts.ItemsSource = productBLL.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(keyword))
                LoadProducts();
            else
                dgProducts.ItemsSource = productBLL.Search(keyword);
        }
        
        private void dgProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgProducts.SelectedItem is Product product)
            {
                AddToCart(product);
            }
        }
        
        private void AddToCart(Product product)
        {
            // Kiểm tra tồn kho
            if (product.Quantity <= 0)
            {
                MessageBox.Show("Sản phẩm đã hết hàng!");
                return;
            }
            
            // Kiểm tra đã có trong giỏ chưa
            var existing = cart.Find(x => x.ProductId == product.ProductId);
            if (existing != null)
            {
                existing.Quantity += 1;
                existing.LineTotal = existing.Quantity * existing.UnitPrice;
            }
            else
            {
                cart.Add(new InvoiceDetail
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Quantity = 1,
                    UnitPrice = product.SellingPrice,
                    LineTotal = product.SellingPrice
                });
            }
            
            UpdateCart();
        }
        
        private void UpdateCart()
        {
            // Chờ page load xong
            if (!isLoaded) return;
            if (dgCart == null || lblSubTotal == null || lblVAT == null || lblTotal == null) return;
            
            dgCart.ItemsSource = null;
            dgCart.ItemsSource = cart;
            
            // Tính tạm tính
            subTotal = 0;
            foreach (var item in cart)
                subTotal += item.LineTotal;
            
            // Tính giảm giá
            string discountText = txtDiscount?.Text ?? "0";
            decimal.TryParse(discountText, out discountPercent);
            if (discountPercent < 0) discountPercent = 0;
            if (discountPercent > 100) discountPercent = 100;
            discountAmount = subTotal * discountPercent / 100;
            
            // Tính VAT 10%
            decimal afterDiscount = subTotal - discountAmount;
            vatAmount = afterDiscount * 0.1m;
            
            // Tổng cộng
            total = afterDiscount + vatAmount;
            
            // Hiển thị
            lblSubTotal.Text = subTotal.ToString("N0") + " VND";
            lblVAT.Text = vatAmount.ToString("N0") + " VND";
            lblTotal.Text = total.ToString("N0") + " VND";
            
            CalculateChange();
        }
        
        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isLoaded) UpdateCart();
        }
        
        private void txtReceived_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isLoaded) CalculateChange();
        }
        
        private void CalculateChange()
        {
            if (lblChange == null || txtReceived == null) return;
            
            string receivedText = txtReceived.Text ?? "0";
            if (decimal.TryParse(receivedText, out decimal received) && received > 0)
            {
                decimal change = received - total;
                if (change >= 0)
                {
                    lblChange.Text = change.ToString("N0") + " VND";
                    lblChange.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    lblChange.Text = "Thiếu " + Math.Abs(change).ToString("N0") + " VND";
                    lblChange.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                lblChange.Text = "0";
            }
        }
        
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dgCart.SelectedItem is InvoiceDetail item)
            {
                cart.Remove(item);
                UpdateCart();
            }
        }
        
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cart.Clear();
            if (txtDiscount != null) txtDiscount.Text = "0";
            if (txtReceived != null) txtReceived.Text = "";
            UpdateCart();
        }
        
        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!");
                return;
            }
            
            // Kiểm tra tiền khách đưa
            string receivedText = txtReceived?.Text ?? "0";
            if (!decimal.TryParse(receivedText, out decimal received) || received < total)
            {
                MessageBox.Show("Tiền khách đưa không đủ!");
                return;
            }
            
            try
            {
                Invoice invoice = new Invoice
                {
                    UserId = LoginWindow.CurrentUser?.UserId ?? 1,
                    CustomerName = txtCustomerName?.Text?.Trim() ?? "",
                    TotalAmount = subTotal,
                    FinalAmount = total,
                    Details = cart
                };
                
                int invoiceId = invoiceBLL.CreateInvoice(invoice);
                
                decimal change = received - total;
                MessageBox.Show($"Thanh toán thành công!\nMã hóa đơn: {invoiceId}\nTiền thừa: {change:N0} VND", "Thông báo");
                
                // Xóa giỏ hàng
                cart = new List<InvoiceDetail>();
                if (txtCustomerName != null) txtCustomerName.Text = "";
                if (txtDiscount != null) txtDiscount.Text = "0";
                if (txtReceived != null) txtReceived.Text = "";
                UpdateCart();
                
                // Refresh danh sách sản phẩm
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
