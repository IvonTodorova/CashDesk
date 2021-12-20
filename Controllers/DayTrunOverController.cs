using AutoMapper;
using CashDesk.Data;
using CashDesk.Data.Attributes;
using CashDesk.Data.Dto;
using CashDesk.Data.Models;
using CashDesk.Data.Repositories;
//using CashDesk.Data.Repositories.DayTurnOverRepos;
using CashDesk.Data.Repositories.ExpenseRepos;
using CashDesk.Data.Repositories.IncomeRepos;
using CashDesk.Data.Repositories.UserRepos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ValueProviders;

namespace CashDesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayTrunOverController:Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly CashDeskDbContext _context;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IExpenseRepository _expenseRepository;

        public DayTrunOverController(IUserRepository userRepository, IMapper mapper, CashDeskDbContext context, IIncomeRepository _incomeRepository, IExpenseRepository _expenseRepository)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._context = context;
            this._incomeRepository = _incomeRepository;
            this._expenseRepository = _expenseRepository;
        }

        [HttpGet]
        [Route("GetDayTurnOver")]
        public ActionResult<DayTurnOver> PostDayTurnOverbyDate([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, DateTime date)
        {
            bool isSessionKyeValid = _userRepository.ValidateSessionKey(sessionKey);
            if (!isSessionKyeValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            if (date.Date == DateTime.MinValue)
            {
                return BadRequest();
            }

            DayTurnOver dateTyrnOver = new DayTurnOver
            {
                SetDateTime = date.Date,
                ReceptionistId = _userRepository.GetUserBySessionKey(sessionKey).Id,
                Incomes = _incomeRepository.GetIncomeByDate(date.Date),
                Expenses = _expenseRepository.GetExpenseByDate(date.Date),
                CurrentBalance = GetDailyBalance(date.Date),
                //get the balance only for today: biginningBalance + vsichki prihodi dnes - vsichki razhodi dnes
                BeginningBalance= GetStartingDayBalance(date),
                //get daily balance (do predniq den vsichki prihodi minus vsichki razhodi)
            };
            
            return dateTyrnOver;
        }
        private decimal GetStartingDayBalance(DateTime date)
        {
            int count = -1;
            DateTime d1 = date.AddDays(count);
           
            var incomebyDate = _context.Incomes.Where(x => x.IncomeDate <= d1);
            var expenseByDate = _context.Expenses.Where(x => x.ExpenseDate <= d1);

            decimal totalValueIncomes = 0;
            decimal totalValueExpenses = 0;

            foreach (var income in incomebyDate)
            {
                //var convertstringToDateTime = d1.Date.ToString("MM/dd/yyyy");

                //if (!String.IsNullOrEmpty(convertstringToDateTime))
                //{
                //    count--;
                //}
                totalValueIncomes += income.Value;
            }
            
            foreach (var expense in expenseByDate)
            {
                totalValueExpenses += expense.Value;
            }
            decimal dailyBalace = totalValueIncomes - totalValueExpenses; 
            return dailyBalace;
        }
        
        private decimal GetDailyBalance(DateTime d1)
        {        
            var todayIncome = _incomeRepository.GetIncomeByDate(d1.Date);
            var todayExpense = _expenseRepository.GetExpenseByDate(d1.Date);

            decimal currentIncome = 0;
            decimal currentExpense = 0;
            decimal totalDailyBalance = 0;
            foreach (var item in todayIncome)
            {
                currentIncome+= item.Value;
            }
            foreach (var item2 in todayExpense)
            {
                currentExpense += item2.Value;
            }
            var getPriviusDayBalance = GetStartingDayBalance(d1);

            totalDailyBalance = getPriviusDayBalance + currentIncome - currentExpense;
            return totalDailyBalance;
        }


        

    }
}
