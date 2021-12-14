﻿using CashDesk.Data;
using CashDesk.Data.Attributes;
using CashDesk.Data.Models;
using CashDesk.Data.Repositories.CategoryRepos;
using CashDesk.Data.Repositories.IncomeRepos;
using CashDesk.Data.Repositories.UserRepos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Http.ValueProviders;

namespace CashDesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController:Controller
    {
        private readonly CashDeskDbContext _context;
        private readonly IIncomeRepository _incomeRepo;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        public IncomeController(CashDeskDbContext _context, IIncomeRepository _incomeRepo, IUserRepository _userRepository, ICategoryRepository _categoryRepository)
        {
            this._context = _context;
            this._incomeRepo = _incomeRepo;
            this._userRepository = _userRepository;
            this._categoryRepository = _categoryRepository;
        }

        [HttpPost]
        [Route("CreateIncome")]
        public ActionResult<Income> CreateIncome([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey,Income income)
        {
            bool isSessionKyeValid = _userRepository.ValidateSessionKey(sessionKey);

            if (!isSessionKyeValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            if (income == null)
            {
                return BadRequest();
            }

            var userBySessionKey = _userRepository.GetUserBySessionKey(sessionKey);
            var category = _categoryRepository.GetCategoryById(income.CategoryId);
            Income newIncome = new Income
            {
               IncomeDate = DateTime.Now.Date,
               Value=income.Value,
               Title=income.Title,
               IncomeUserId=userBySessionKey.Id,
               CategoryId = category.Id,
            };

            _incomeRepo.CreateDailyIncome(newIncome);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("EditIncome")]
        public ActionResult EditIncome([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey, Income income)
        {
            bool isSessionKyeValid = _userRepository.ValidateSessionKey(sessionKey);

            if (!isSessionKyeValid)
            {
                return BadRequest("Invalid Seesion Key.");
            }

            try
            {
                _incomeRepo.EditIncome(income);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }        

        }
        [HttpGet]
        [Route("GetAllIncomes")]
        public ActionResult GetAllIncomes()
        {
            List<Income> incomes = new List<Income>();

            var allIncomes = _incomeRepo.GetAllIncomes();

            foreach (var item in allIncomes)
            {
                incomes.Add(item);
            }

            return Ok(incomes);
        }


    }
}
