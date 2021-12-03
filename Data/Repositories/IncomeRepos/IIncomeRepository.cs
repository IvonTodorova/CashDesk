using CashDesk.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Repositories.IncomeRepos
{
    public interface IIncomeRepository
    {
        public void CreateDailyIncome(Income income);
        public ICollection<Income> GetAllIncomes();
        public Income GetIncomesById(int Id);
        public void EditIncome(Income income);


    }
}
