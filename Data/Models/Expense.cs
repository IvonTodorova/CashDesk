using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Title { get; set; }
        public int ReceiverExpenseId { get; set; }
        #nullable enable
        public virtual User? ReceiverExpense { get; set; }
        public int CreaterExpenseId { get; set; }
        #nullable enable
        public virtual User? CreaterExpense { get; set; }
#nullable enable
        public int DayTurnOverId  { get; set; }
        public virtual DayTurnOver? DayTurnOver { get; set; }
        public int  CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
