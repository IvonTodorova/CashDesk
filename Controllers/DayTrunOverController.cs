using AutoMapper;
using CashDesk.Data;
using CashDesk.Data.Attributes;
using CashDesk.Data.Models;
using CashDesk.Data.Repositories;
using CashDesk.Data.Repositories.DayTurnOverRepos;
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
    public class DayTrunOverController:Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly CashDeskDbContext _context;
        private readonly IDayTurnOverRepository _dayTurnOverRepo;
        public DayTrunOverController(IUserRepository userRepository, IMapper mapper, CashDeskDbContext context, IDayTurnOverRepository _dayTurnOverRepo)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._context = context;
            this._dayTurnOverRepo = _dayTurnOverRepo;
        }

        [HttpPost]
        [Route("GetDayTrunOver")]
        public ActionResult<DayTurnOver> GetDayTurnOverbyDate([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, DateTime date)
        {
            bool isSessionKyeValid = _userRepository.ValidateSessionKey(sessionKey);
            if (!isSessionKyeValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            if (date == DateTime.MinValue)
            {
                return BadRequest();
            }

            var dateTyrnOver = _dayTurnOverRepo.GetDayTurnOverByDate(date);
            return dateTyrnOver;
        }


    }
}
