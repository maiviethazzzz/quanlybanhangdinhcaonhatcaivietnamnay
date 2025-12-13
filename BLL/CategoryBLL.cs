using System.Collections.Generic;
using QuanLyBanHang.DAL;
using QuanLyBanHang.MODEL;

namespace QuanLyBanHang.BLL
{
    public class CategoryBLL
    {
        private CategoryDAL categoryDAL = new CategoryDAL();
        
        public List<Category> GetAll()
        {
            return categoryDAL.GetAll();
        }
        
        public int Insert(Category category)
        {
            if (string.IsNullOrEmpty(category.CategoryName))
                throw new System.Exception("Tên danh mục không được trống!");
            
            return categoryDAL.Insert(category);
        }
        
        public void Update(Category category)
        {
            if (string.IsNullOrEmpty(category.CategoryName))
                throw new System.Exception("Tên danh mục không được trống!");
            
            categoryDAL.Update(category);
        }
        
        public void Delete(int categoryId)
        {
            categoryDAL.Delete(categoryId);
        }
    }
}
