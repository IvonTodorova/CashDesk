using CashDesk.Data;
using CashDesk.Data.Attributes;
using CashDesk.Data.Dto;
using CashDesk.Data.Models;
using CashDesk.Data.Repositories.CategoryRepos;
using CashDesk.Data.Repositories.ExpenseRepos;
using CashDesk.Data.Repositories.UserRepos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ValueProviders;

namespace CashDesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController:Controller
    {
        private readonly CashDeskDbContext _context;
        private readonly IExpenseRepository _expenseRepo;
        private readonly IUserRepository _userRepo;
        private readonly ICategoryRepository _categoryRepository;
        public ExpenseController(CashDeskDbContext _context, IExpenseRepository _expenseRepo, IUserRepository _userRepo, ICategoryRepository _categoryRepository)
        {
            this._context = _context;
            this._expenseRepo = _expenseRepo;
            this._userRepo = _userRepo;
            this._categoryRepository = _categoryRepository;
        }

      [HttpPost]
        [Route("CreateExpense")]
        public ActionResult<Expense> CreateExpense([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, Expense expense)
        {
            bool isSessionKyeValid = _userRepo.ValidateSessionKey(sessionKey);
            if (expense==null)
            {
                return BadRequest();
            }
            if (!isSessionKyeValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            var userBySessionKey = _userRepo.GetUserBySessionKey(sessionKey);
            var receiver = _context.Users.FirstOrDefault(x => x.Id == expense.ReceiverExpenseId);
            var category = _categoryRepository.GetCategoryById(expense.CategoryId);

            Expense newExpense = new Expense
            {
                Value = expense.Value,
                ExpenseDate = DateTime.Now.Date,
                Title = expense.Title,
                ReceiverExpenseId = receiver?.Id,
                CreaterExpenseId = userBySessionKey.Id,
                CategoryId= category.Id,              
            };

            _expenseRepo.CreateDailyOutcome(newExpense);
            _context.SaveChanges();

            return Ok(newExpense);
        }

        [HttpPost]
        [Route("EditExpense")]
        public ActionResult EditExpense([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, Expense expense)
        {
            bool isSessionKeyValid = _userRepo.ValidateSessionKey(sessionKey);
            if (!isSessionKeyValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            try
            {
                _expenseRepo.EditExpense(expense);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);               
            }
        }

        [HttpGet]
        [Route("GetAllExpenses")]

        public ActionResult GetAllExpenses()
        {
            List<Expense> expenses = new List<Expense>();

            var allExpenses = _expenseRepo.GetAllExpenses();
            foreach (var item in allExpenses)
            {
                expenses.Add(item);
            }
            return Ok(expenses);
        }

        [HttpPost]
        [Route("GetCategoryExpenseFilter")]
        public ActionResult<ICollection<Expense>> GetCategoryExpenseFilter([ValueProvider(typeof(HeaderValueProviderFactory<string>))] Category category,string sessionKey, DateTime startDate, DateTime endDate,string userName)
        {
            bool isSessionKeyValid = _userRepo.ValidateSessionKey(sessionKey);
            if (!isSessionKeyValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            FilterArgs filter = new FilterArgs
            {
                StartDate = startDate,
                EndDate = endDate,
                CategoryId = _categoryRepository.GetCategoryByName(category).Id,
            };

            var currentDatesExpenses = _expenseRepo.FilterByUser(filter,userName);
           
            return Ok(currentDatesExpenses);
        }
        [HttpPost]
        [Route("GetByUserExpenseFilter")]
        public ActionResult<ICollection<Expense>> GetByUserExpenseFilter([ValueProvider(typeof(HeaderValueProviderFactory<string>))] Category category, string sessionKey, DateTime startDate, DateTime endDate, string userName)
        {
            bool isSessionKeyValid = _userRepo.ValidateSessionKey(sessionKey);
            if (!isSessionKeyValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            FilterArgs filter = new FilterArgs
            {
                StartDate = startDate,
                EndDate = endDate,
                CategoryId = _categoryRepository.GetCategoryByName(category).Id,
            };

            var currentIncomeFilter = _expenseRepo.FilterByUser(filter, userName);
            return Ok(currentIncomeFilter);
        }

        [HttpGet]
        [Route("GetIncomesByTitle")]
        public ActionResult<ICollection<Expense>> GetIncomesByTitle([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, string title)
        {
            bool isSessionKeyValid = _userRepo.ValidateSessionKey(sessionKey);
            if (!isSessionKeyValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            List<Expense> expenses = new List<Expense>();
            var incomesByTite = _expenseRepo.GetExpenseByTitle(title);
            foreach (var item in incomesByTite)
            {
                expenses.Add(item);
            }

            return Ok(expenses);
        }
    }
}
