using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Models
{
    public class Income
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Value { get; set; }
        public DateTime IncomeDate { get; set; }
        public string Title { get; set; }
        public int IncomeUserId { get; set; }
        public virtual User IncomeUser { get; set; }
        public int CategoryId { get; set; }
        #nullable enable
        public virtual Category? Category { get; set; }
        #nullable enable
    }
}
 