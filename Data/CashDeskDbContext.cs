using CashDesk.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk.Data
{
    public class CashDeskDbContext : DbContext
    {
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public CashDeskDbContext(DbContextOptions<CashDeskDbContext> options)
              : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Category> Categories { get; set; }
        
    }
}
