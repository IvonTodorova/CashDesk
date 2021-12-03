using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Models
{
    public class DayTurnOver
    {
        public int Id { get; set; }
        public int ReceptionistId { get; set; }
        public virtual User Receptionist { get; set; }
        public DateTime SetDateTime { get; set; }
        public ICollection<Income> Incomes { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
