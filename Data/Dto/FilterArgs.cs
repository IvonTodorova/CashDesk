using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Dto
{
    public class FilterArgs
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
        //TODO start date
        //TODO end date
        //TODO category ID
        //...
    }
}
