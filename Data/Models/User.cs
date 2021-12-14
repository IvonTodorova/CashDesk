using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public virtual int Salarie { get; set; }
        public string? BankAccount { get; set; }
        public string AutificationKey { get; set; }
        public string SessionKey { get; set; }
        public bool IsActive { get; set; }
    }
}
