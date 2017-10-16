using System;

namespace BankAccounts.Models
{
    public class Transaction : BaseEntity
    {
        public int Id { get; set;}
        public string Type { get; set; }
        public float Amount { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set;}
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}