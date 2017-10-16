using System;
using System.ComponentModel.DataAnnotations;
namespace BankAccounts.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set;}
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
