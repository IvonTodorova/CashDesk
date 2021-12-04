using CashDesk.Data.Dto;
using CashDesk.Data.Models;
using CashDesk.Data.Repositories.DayTurnOverRepos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashDesk.Data.Repositories.UserRepos
{
    public class UserRepository : IUserRepository
    {
        private readonly CashDeskDbContext _context;
        private readonly IDayTurnOverRepository _dayTurnOverRepository;

        public UserRepository(CashDeskDbContext context, IDayTurnOverRepository _dayTurnOverRepository)
        {
            this._context = context;
            this._dayTurnOverRepository = _dayTurnOverRepository;
        }
        public bool CheckUser(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }
            return _context.Users.Any(x => x.Name == userName);
        }

        public void CreateUser(User item)
        {
            if (item==null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            _context.Users.Add(item);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            user.IsActive = false;
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.OrderByDescending(x=>x.Name!=null);
        }

        public User GetById(int id)
        {
            User user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        public User Login(string userName, string authificationKey)
        {
            User user = _context.Users.FirstOrDefault(x => x.Name == userName);

            if (user.AutificationKey!=authificationKey)
            {
                throw new ArgumentException("Invalid user password");
            }

            //creates guid random string without hyphen
            user.SessionKey = Guid.NewGuid().ToString("N");
            //User is logged in
            _context.SaveChanges();

            return user;
        }

        public void LogoutUser(int userId)
        {    
            User userLogOut = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (userLogOut == null)
            {
                throw new ArgumentException();
            }
            userLogOut.SessionKey = null;
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateUser(UserDto updatedUser)
        {
            User user = _context.Users.FirstOrDefault(x => x.Id == updatedUser.Id);
            if (user==null)
            {
                throw new ArgumentNullException("The targetted user is not found.");
            }

            if (updatedUser.Name!=null)
                 user.Name = updatedUser.Name;
            if (updatedUser.LastName != null)
                user.LastName = updatedUser.LastName;
            if (updatedUser.Position != null)
                user.Position = updatedUser.Position;
            if (updatedUser.Salarie != 0)
                user.Salarie = (int)updatedUser.Salarie;

            _context.SaveChanges();
        }

        public bool ValidateSessionKey(string sessionKey)
        {
            return _context.Users.Any(u => u.SessionKey == sessionKey);
        }
        public User GetUserBySessionKey(string sessionKey)
        {
            return _context.Users.FirstOrDefault(x => x.SessionKey == sessionKey);
        }

        public decimal GetDailyBalance(int count) 
        {
            DateTime d1 = DateTime.Now;

            //TODO check if there is an existing dayTurnOver

            if (_dayTurnOverRepository.GetDayTurnOverByDate(d1.AddDays(count))== null)
            {              
                count--;
                return GetDailyBalance(count); 
            }

            var day = _dayTurnOverRepository.GetDayTurnOverByDate(d1.AddDays(count));
            var incomes = day.Incomes;
            decimal todaysIncomes = 0;

            var expenses = day.Expenses;
            decimal todaysExpenses = 0;
            foreach (var income in incomes)
            {
                todaysIncomes += income.Value;
            }
            foreach (var expense in expenses)
            {
                todaysExpenses += expense.Value;
            }
            var dayBalance = todaysIncomes - todaysExpenses;
            return dayBalance;
        }
}
}
