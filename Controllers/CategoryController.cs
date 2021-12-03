using CashDesk.Data;
using CashDesk.Data.Attributes;
using CashDesk.Data.Models;
using CashDesk.Data.Repositories.CategoryRepos;
using CashDesk.Data.Repositories.UserRepos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CashDesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly CashDeskDbContext _context;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUserRepository _userRepository;

        public CategoryController(CashDeskDbContext _context, ICategoryRepository _categoryRepo, IUserRepository userRepository)
        {
            this._context = _context;
            this._categoryRepo = _categoryRepo;
            this._userRepository = userRepository;
        }

        [HttpPost]
        [Route("CreateCategory")]
        public ActionResult<Category> CreateCategory([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, Category newCategory)
        {
            bool isSessionKyeValid = _userRepository.ValidateSessionKey(sessionKey);
            if (!isSessionKyeValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            Category category = new Category
            {
                Description = newCategory.Description,
                Name = newCategory.Name,
            };

            _categoryRepo.CreateCategory(category);
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpPost]
        [Route("EditCategory")]
        public ActionResult EditCategory([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, Category category)
        {
            bool isSessionKyeValid = _userRepository.ValidateSessionKey(sessionKey);
            if (!isSessionKyeValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            _categoryRepo.EditCategory(category);
            _context.SaveChanges();
            return Ok(category);
        }

        [HttpPost]
        [Route("DeleteCategory")]
        public ActionResult DeleteCategory([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, Category category)
        {
            bool isSessionKyeValid = _userRepository.ValidateSessionKey(sessionKey);
            if (!isSessionKyeValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            _categoryRepo.DeleteCategory(category.Id);
            _context.SaveChanges();
            return Ok(category);
        }

        [HttpGet]
        [Route("GetAllCategoriesBy")]
        public ActionResult GetAllCategory([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            List<Category> categories = new List<Category>();

            var getallCategories = _categoryRepo.GetAllCategoriesBy();

            foreach (var item in getallCategories)
            {
                categories.Add(item);
            }

            return Ok(categories);
        }



    }
}
