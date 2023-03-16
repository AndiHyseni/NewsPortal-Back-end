using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Intefaces
{
    public interface ICategories
    {
        Task<List<Category>> getAllCategories();
        Task<Category> GetCategoryById(int id);
        Task addCategory(Category category);
       Task DeleteCategory(int id);
        //public void editCAtegory(Category category);
    }
}
