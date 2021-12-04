using CashDesk.Data;
using CashDesk.Data.Attributes;
using CashDesk.Data.Models;
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
        public ExpenseController(CashDeskDbContext _context, IExpenseRepository _expenseRepo, IUserRepository _userRepo)
        {
            this._context = _context;
            this._expenseRepo = _expenseRepo;
            this._userRepo = _userRepo;
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
            var receiver = _context.Users.FirstOrDefault(x => x.Name == expense.ReceiverExpense.Name && x.LastName == expense.ReceiverExpense.LastName);           

            Expense newExpense = new Expense
            {
                Value = expense.Value,
                ExpenseDate = new DateTime().Date,
                Title = expense.Title,
                ReceiverExpenseId=receiver.Id,
                CreaterExpenseId = userBySessionKey.Id,
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

      //  [HttpGet]
      //[Route(GetAllExpenses)]

      //public ActionResult GetAllExpenses()
      // {

      // }
    }
}
