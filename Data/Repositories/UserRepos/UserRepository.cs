using CashDesk.Data.Dto;
using CashDesk.Data.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CashDesk.Data.Repositories.UserRepos
{
    public class UserRepository : IUserRepository
    {
        private readonly CashDeskDbContext _context;
        //private readonly IDayTurnOverRepository _dayTurnOverRepository;

        public UserRepository(CashDeskDbContext context)
        {
            this._context = context;
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
        public string HashPassword(string password)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }          

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
        
}

