using CashDesk.Data.Models;
using CashDesk.Data.Repositories.UserRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CashDesk.Data.Repositories.CategoryRepos
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly CashDeskDbContext _context;

        public CategoryRepository(CashDeskDbContext context)
        {        
            this._context = context;
        }

        public void CreateCategory(Category category)
        {
            if (category==null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Add(category);
            _context.SaveChanges();

        }

        public void DeleteCategory(int categoryId)
        {
            var category =_context.Categories.FirstOrDefault(x => x.Id == categoryId);

            _context.Remove(category);
        }

        public void EditCategory(Category category)
        {
            var editCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (category!=null)
            {
                editCategory.Name = category.Name;
                editCategory.Description = category.Description;
            }
        }

        public ICollection<Category> GetAllCategories()
        {
            var categories = _context.Categories.ToList();

            return categories;
        }
        public Category GetCategoryById(int Id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == Id);

            return category;
        }
        public Category GetCategoryByName(Category category)
        {
            var categoryByName = _context.Categories.FirstOrDefault(x => x.Name == category.Name);
            return categoryByName;
        }
}
}
