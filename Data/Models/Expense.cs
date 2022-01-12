using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Value { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Title { get; set; }
        public int? ReceiverExpenseId { get; set; }
        #nullable enable
        public virtual User? ReceiverExpense { get; set; }
        public int? CreaterExpenseId { get; set; }
        #nullable enable
        public virtual User? CreaterExpense { get; set; }
        #nullable enable
        public int CategoryId { get; set; }
        #nullable enable
        public virtual Category? Category { get; set; }
        #nullable enable
    }
}
