using PizzaStar.Data;
using PizzaStar.Interfaces;
using PizzaStar.Models.Pages;
using PizzaStar.Models;
using Microsoft.EntityFrameworkCore;

namespace PizzaStar.Repository
{
    public class CategoryRepository : ICategory
    {
        private readonly ApplicationContext _applicationContext;
        public CategoryRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public PagedList<Category> GetAllCategories(QueryOptions options)
        {
            return new PagedList<Category>(_applicationContext.Categories, options);
        }


        public async Task AddCategoryAsync(Category category)
        {
            await _applicationContext.Categories.AddAsync(category);
            await _applicationContext.SaveChangesAsync();
        }


        public async Task DeleteCategoryAsync(Category category)
        {
            _applicationContext.Categories.Remove(category);
            await _applicationContext.SaveChangesAsync();
        }


        public async Task EditCategoryAsync(Category category)
        {
            _applicationContext.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _applicationContext.Entry(category).Property(e => e.DateOfPublication).IsModified = false;
            await _applicationContext.SaveChangesAsync();
        }


        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _applicationContext.Categories.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _applicationContext.Categories.AsNoTracking().ToListAsync();
        }
    }

}
