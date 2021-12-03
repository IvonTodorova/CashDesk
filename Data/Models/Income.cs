using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Models
{
    public class Income
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateTime IncomeDate { get; set; }
        public string Title { get; set; }
        public int IncomeUserId { get; set; }
        public virtual User IncomeUser { get; set; }
        public int DayTurnOverId { get; set; }
        public virtual DayTurnOver? DayTurnOver { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
 