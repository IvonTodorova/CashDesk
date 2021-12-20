using CashDesk.Data.Dto;
using CashDesk.Data.Models;
using System;
using System.Collections.Generic;

namespace CashDesk.Data.Repositories.ExpenseRepos
{
    public interface IExpenseRepository
    {
        public void CreateDailyOutcome(Expense expense);
        public Expense GetExpenseById(Expense expense);

        public void EditExpense(Expense expense);
        public ICollection<Expense> GetExpenseByDate(DateTime date);
        public ICollection<Expense> GetAllExpenses();
        public ICollection<Expense> Filter(FilterArgs filterArgs);
        public ICollection<Expense> FilterByUser(FilterArgs filterArgs, string userName);
        public ICollection<Expense> GetExpenseByTitle(string title);


    }
}
