using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BankAccounts.Models;
using System.Linq;
using System.Collections;

namespace BankAccounts.Controllers
{
    public class AccountController : Controller
    {
        private BankAccountsContext _context;

        public AccountController(BankAccountsContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("account")]
        public IActionResult Account()
        {
            string firstName = HttpContext.Session.GetString("FirstName");
            ViewBag.FirstName = firstName;
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrWhiteSpace(firstName) || userId == null) {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Registration");
            }

            List<Account> accounts = _context.Accounts.Where(x => x.UserId == userId).ToList();

            Account account = accounts[0];
            ViewBag.Balance = account.Balance;

            // List<Transaction> transactions = _context.Transactions.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).Take(num_transactions).ToList();
            List<Transaction> transactions = _context.Transactions.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).ToList();
            ViewBag.Transactions = transactions;

            ViewBag.Error = HttpContext.Session.GetString("Error");
            return View("Account");
        }

        [HttpPost]
        [Route("DepWith")]
        public IActionResult DepWith(string activity, float amount)
        {

            Console.WriteLine("activity: " + activity);
            Console.WriteLine("amount: " + amount);

            int userId = (int) HttpContext.Session.GetInt32("UserId");
            Account account = _context.Accounts.SingleOrDefault(x => x.UserId == userId);

            Transaction transaction = new Transaction {
                Amount = amount,
                AccountId = account.Id,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            float balance = account.Balance;

            if(String.Equals("Deposit", activity, StringComparison.OrdinalIgnoreCase)) {
                balance += amount;
                transaction.Type = "Deposit";
            } else if (String.Equals("Withdraw", activity, StringComparison.OrdinalIgnoreCase) && balance >= amount) {
                balance -= amount;
                transaction.Type = "Withdraw";
            } else if (String.Equals("Withdraw", activity, StringComparison.OrdinalIgnoreCase) && balance < amount) {
                HttpContext.Session.SetString("Error", "Attempting to withdraw more than the current balance");
                return RedirectToAction("Account");
            }

            _context.Transactions.Add(transaction);

            account.Balance = balance;
            account.UpdatedAt = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("Account");
        }

        [HttpGet]
        [Route("Logoff")]
        public IActionResult Logoff()
        {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Registration");
        }
    }
}
