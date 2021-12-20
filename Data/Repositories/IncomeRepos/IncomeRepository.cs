using CashDesk.Data.Dto;
using CashDesk.Data.Models;
using CashDesk.Data.Repositories.UserRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Repositories.IncomeRepos
{
    public class IncomeRepository:IIncomeRepository
    {
        private readonly CashDeskDbContext _context;
        private readonly IUserRepository _userRepository;
        public IncomeRepository(CashDeskDbContext _context, IUserRepository _userRepository)
        {
            this._context = _context;
            this._userRepository = _userRepository;
        }
        public void CreateDailyIncome(Income income)
        {
            if (income == null)
            {
                throw new ArgumentNullException(nameof(income));
            }                     
            _context.Add(income);
            _context.SaveChanges();
        } 
        public ICollection<Income> GetAllIncomes()
        {
            var incomes = _context.Incomes.ToList();

            return incomes;
        }
        public Income GetIncomesById(int Id)
        {
            var income = _context.Incomes.FirstOrDefault(x=>x.Id==Id);

            return income;
        }

        public ICollection<Income> GetIncomeByDate(DateTime date)
        {
            var incomesByDate = _context.Incomes.Where(d => d.IncomeDate == date).ToList();  
            
            return incomesByDate;
        }
        public void EditIncome(Income income)
        {
            var item = _context.Incomes.FirstOrDefault(x => x.Id == income.Id);

            if (item.Value != income.Value)
                item.Value = income.Value;

            if (item.Title != income.Title)
                item.Title = income.Title;

            if (item.Category != income.Category)
                item.Category = income.Category;

            if (item.IncomeUserId != income.IncomeUserId)
                item.IncomeUserId = income.IncomeUserId;

            _context.SaveChanges();
        }
        public ICollection<Income> Filter(FilterArgs filterArgs)
        {
            //|TODO check if null values are provided to filter arguments
            //TODO if null do not take in cosideration         
            var filteredExpenses = _context.Incomes.Where(e => e.IncomeDate >= filterArgs.StartDate
                                                            || e.IncomeDate <= filterArgs.EndDate
                                                            && e.CategoryId == filterArgs.CategoryId);
            return filteredExpenses.ToList();
        }
        public ICollection<Income> FilterByUser(FilterArgs filterArgs,string userName)
        {
            //|TODO check if null values are provided to filter arguments
            //TODO if null do not take in cosideration
            var user = _userRepository.GetUserByName(userName);
            var filteredExpenses = _context.Incomes.Where(e => e.IncomeDate >= filterArgs.StartDate
                                                            || e.IncomeDate <= filterArgs.EndDate
                                                            && e.CategoryId == filterArgs.CategoryId && e.IncomeUserId==user.Id);
            return filteredExpenses.ToList();
        }
        public ICollection<Income> GetIncomesByTitle(string title)
        {
            var incomesByTitle = _context.Incomes.Where(x => x.Title == title);
            return incomesByTitle.ToList();
        }

    }
}
