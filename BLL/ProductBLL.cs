using System.Collections.Generic;
using QuanLyBanHang.DAL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.BLL
{
    public class ProductBLL
    {
        private ProductDAL productDAL = new ProductDAL();
        
        public List<Product> GetAll()
        {
            return productDAL.GetAll();
        }
        
        public List<Product> Search(string keyword)
        {
            return productDAL.Search(keyword);
        }
        
        public int Insert(Product product)
        {
            // Kiểm tra dữ liệu
            if (string.IsNullOrEmpty(product.ProductName))
                throw new System.Exception("Tên sản phẩm không được trống!");
            if (product.CostPrice < 0 || product.SellingPrice < 0)
                throw new System.Exception("Giá không được âm!");
            
            return productDAL.Insert(product);
        }
        
        public void Update(Product product)
        {
            if (string.IsNullOrEmpty(product.ProductName))
                throw new System.Exception("Tên sản phẩm không được trống!");
            
            productDAL.Update(product);
        }
        
        public void Delete(int productId)
        {
            productDAL.Delete(productId);
        }
        
        // Cảnh báo tồn kho thấp
        public List<Product> GetLowStock(int minQuantity = 10)
        {
            var all = productDAL.GetAll();
            var lowStock = new List<Product>();
            foreach (var p in all)
            {
                if (p.Quantity <= minQuantity)
                    lowStock.Add(p);
            }
            return lowStock;
        }
    }
}
