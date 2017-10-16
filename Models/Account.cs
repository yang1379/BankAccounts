using System;

namespace BankAccounts.Models
{
    public class Account : BaseEntity
    {
        public int Id { get; set;}
        public int UserId { get; set;}
        public float Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}