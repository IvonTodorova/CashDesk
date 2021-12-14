using CashDesk.Data.Models;
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
        public IncomeRepository(CashDeskDbContext _context)
        {
            this._context = _context;
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


    }
}
