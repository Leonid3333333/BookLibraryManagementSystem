using System;

namespace BookLibraryManagementSystem.Models
{
    public class TransactionLog
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public int MemberId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
