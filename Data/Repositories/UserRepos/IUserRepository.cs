using CashDesk.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashDesk.Data.Dto;

namespace CashDesk.Data.Repositories.UserRepos
{
    public interface IUserRepository
    {
        void SaveChanges();
        IEnumerable<User> GetAllUsers();
        User GetById(int id);
        bool CheckUser(string userName);
        User Login(string userName, string authificationKey);
        void CreateUser(User item);
        void LogoutUser(int userId);
        void DeleteUser(int userId);
        void UpdateUser(UserDto updatedUser);
        bool ValidateSessionKey(string sessionKey);
        public User GetUserBySessionKey(string sessionKey);
        //public decimal GetDailyBalance(int count);
        public string HashPassword(string password);
        public User GetUserByName(string userName);

    }
}
