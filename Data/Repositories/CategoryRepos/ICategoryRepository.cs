using CashDesk.Data.EFRepositories;
using CashDesk.Data.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Repositories.CategoryRepos
{
    public interface ICategoryRepository
    {
        public void CreateCategory(Category category);
        public void DeleteCategory(int categoryId);
        public void EditCategory(Category category);
        public ICollection<Category> GetAllCategories();
        public Category GetCategoryById(int Id);
        public Category GetCategoryByName(Category category);


    }
}
