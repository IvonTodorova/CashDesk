using CashDesk.Data.Models;

namespace CashDesk.Data.Repositories.ExpenseRepos
{
    public interface IExpenseRepository
    {
        public void CreateDailyOutcome(Expense expense);
        public Expense GetExpenseById(Expense expense);

        public void EditExpense(Expense expense);
    }
}
