using Portal.Database;
using Portal.Intefaces;
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class CategoryRepository : ICategories
    {
        public AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> getAllCategories()
        {
            try
            {
                var categ = _context.categories.ToList();

                return categ;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task addCategory(Category category)
        {
            try
            {
                if (category.CategoryId != 0)
                {
                    _context.categories.Update(category);
                }
                else
                {
                    _context.categories.Add(category);
                }

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.categories.FindAsync(id);
                return category;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteCategory(int id)
        {
            try
            {
                var category = _context.categories.Where(x => x.CategoryId == id).FirstOrDefault();

                _context.categories.Remove(category);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {

                throw Ex;

            }
        }
    }
}
