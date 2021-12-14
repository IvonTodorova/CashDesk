using CashDesk.Data.Models;
using System;
using System.Collections.Generic;

namespace CashDesk.Data.Dto
{
    public class DayTurnOver
    {
        public int ReceptionistId { get; set; }
        public virtual User Receptionist { get; set; }
        public DateTime SetDateTime { get; set; }
        public ICollection<Income> Incomes { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
