//using CashDesk.Data.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CashDesk.Data.Repositories.DayTurnOverRepos
//{
//    public class DayTurnOverRepository : IDayTurnOverRepository
//    {

//        private readonly CashDeskDbContext _context;
//        public DayTurnOverRepository(CashDeskDbContext context)
//        {
//            this._context = context;
//        }  

//       
//        public void CreateDayTurnOver(DayTurnOver dayTurnOver)
//        {
//            if (dayTurnOver == null)
//            {
//                throw new ArgumentNullException(nameof(dayTurnOver));
//            }

//            _context.Add(dayTurnOver);
//            _context.SaveChanges();
//        }

//    }
//}
