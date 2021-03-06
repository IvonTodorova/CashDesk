using AutoMapper;
using CashDesk.Data;
using CashDesk.Data.Attributes;
using CashDesk.Data.Dto;
using CashDesk.Data.Models;
using CashDesk.Data.Repositories;
using CashDesk.Data.Repositories.UserRepos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ValueProviders;

namespace CashDesk.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly CashDeskDbContext _context;
        public UserController(IUserRepository userRepository, IMapper mapper, CashDeskDbContext context)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._context = context;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public ActionResult<UserDto> RegisterUser([FromBody] UserDto userDto)
        {

            User user = new User()
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Position = userDto.Position,
                Salarie = (int)userDto.Salarie,
                BankAccount = userDto.BankAccount,
                AutificationKey =_userRepository.HashPassword(userDto.AutificationKey),
                IsActive = true,
            };
            _userRepository.CreateUser(user);
            _userRepository.SaveChanges();

            UserDto updatedUserDto = _mapper.Map<UserDto>(user);
            return Ok(updatedUserDto);
        }

        [HttpPost]
        [Route("LogInUser")]
        public ActionResult<UserDto> LogInUser([FromBody] UserDto userDto)
        {
            if (userDto.Name == null || userDto.AutificationKey == null || _userRepository.CheckUser(userDto.Name) == false)
            {
                return BadRequest("Invalid Account or Password");
            }

            try
            {
                var loggedUser = _userRepository.Login(userDto.Name, userDto.AutificationKey);
                var updatedUserDto = _mapper.Map<UserDto>(loggedUser);
               
                _context.SaveChanges();

                return Ok(updatedUserDto);               
            }
            catch (Exception)
            {
                return BadRequest("Invalid Account or Password");
            }
        }

        [HttpGet]
        [Route("LogOut")]
        public ActionResult LogoutUser([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, int userId)
        {
            bool isSessionKeyValid = _userRepository.ValidateSessionKey(sessionKey);
            if (!isSessionKeyValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            try
            {
                _userRepository.LogoutUser(userId);
                return Ok();
            }
            catch (Exception e)
            {

                return NotFound(e.Message);
            }
        }


        [HttpGet]
        [Route("DeleteUser")]
        public ActionResult DeleteUser([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, int userId)
        {
            bool isSessionKeyValid = _userRepository.ValidateSessionKey(sessionKey);
            if (!isSessionKeyValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            try
            {
                _userRepository.DeleteUser(userId);
                return Ok();
            }
            catch (Exception e)
            {

                return NotFound(e);
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        public ActionResult UpdateUser([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, [FromBody] UserDto updatedUser) 
        {
            bool isSessionKeyValid = _userRepository.ValidateSessionKey(sessionKey);
            if (!isSessionKeyValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            try
            {
                _userRepository.UpdateUser(updatedUser);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        } 

    }
}
