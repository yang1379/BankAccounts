using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using BankAccounts.Models;
using System.Linq;

namespace BankAccounts.Controllers
{
    public class RegistrationController : Controller
    {
 
        private BankAccountsContext _context;

        public RegistrationController(BankAccountsContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Error = HttpContext.Session.GetString("Error");
            return View("Registration");
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                Console.WriteLine(model.FirstName);
                Console.WriteLine(model.LastName);
                Console.WriteLine(model.Email);
                Console.WriteLine(model.Password);

                User userExist = _context.Users.SingleOrDefault(x => x.Email == model.Email);

                if(userExist != null) {
                    HttpContext.Session.SetString("Error", String.Format("Email {0} already registered", model.Email));
                    return RedirectToAction("Index");
                }

                User user = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                User newUser = _context.Users.SingleOrDefault(x => x.Email == model.Email);

                HttpContext.Session.SetString("FirstName",newUser.FirstName);
                HttpContext.Session.SetString("LastName", newUser.LastName);
                HttpContext.Session.SetString("Email", newUser.Email);
                HttpContext.Session.SetInt32("UserId", newUser.Id);

                Account account = new Account {
                    UserId = newUser.Id,
                    Balance = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Accounts.Add(account);
                _context.SaveChanges();

            }

            return RedirectToAction("Account", "Account");
        }

        [HttpGet]
        [Route("login_page")]
        public IActionResult LoginPage() {
            return View("Login");
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(RegisterViewModel model)
        {
            if(!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Password)) {
                Console.WriteLine(model.Email);
                Console.WriteLine(model.Password);

                List<User> users = _context.Users.Where(x => x.Email == model.Email).ToList();
                User user = users[0];

                HttpContext.Session.SetString("FirstName",user.FirstName);
                HttpContext.Session.SetString("LastName", user.LastName);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("UserId", user.Id);

                var Hasher = new PasswordHasher<User>();
                // Pass the user object, the hashed password, and the PasswordToCheck
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, model.Password))
                {
                    //Handle success
                    return RedirectToAction("Account", "Account");
                }
                else {
                    return View("Login");
                }
            }

            return View("Login");

            // if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) {
            //     ViewBag.Errors = new List<string>{
            //         "Invalid Name or Password"
            //     };
            //     return View("Login");
            // }

            
            // if(ModelState.IsValid)
            // {
            //     string insertString = String.Format("insert into user (first_name, last_name, email, password) values (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", user.FirstName, user.LastName, user.Email, user.Password);
            //     _dbConnector.Execute(insertString);
            //     return View("Success");
            // }

            // ViewBag.ModelFields = ModelState.Values;
            
        }


    }
}
