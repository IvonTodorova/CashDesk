﻿using CashDesk.Data.Models;
using CashDesk.Data.Repositories.ExpenseRepos;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CashDesk.Data.Repositories
{
    public class ExpenseRepository:IExpenseRepository
    {
        private readonly CashDeskDbContext _context;
        public ExpenseRepository(CashDeskDbContext _context)
        {
            this._context = _context;
        }
        public void CreateDailyOutcome(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            _context.Add(expense);
            _context.SaveChanges();
        }
        public Expense GetExpenseById(Expense expense)
        {
            var expens = _context.Expenses.FirstOrDefault(x => x.Id == expense.Id);
            return expens;
        }
        public void EditExpense(Expense expense) 
        {
            var expens = _context.Expenses.FirstOrDefault(x => x.Id == expense.Id);
            if (expens.Title!=expense.Title)
             expens.Value = expense.Value;

            if (expens.Title != expense.Title)
                expens.Value = expense.Value;

            if (expens.Category != expense.Category)
                expens.Category = expense.Category;

            if (expens.ReceiverExpenseId != expense.ReceiverExpenseId)
                expens.ReceiverExpenseId = expense.ReceiverExpenseId;

            _context.SaveChanges();
        }     
        public ICollection<Expense> GetAllExpenses()
        {
            var expenses = _context.Expenses.ToList();
            return expenses;
        }
    }
}