using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryManagementSystem.Models
{
    public class TransactionLog
    {
        public int Id { get; set; } // Primary Key
        public string ISBN { get; set; }
        public int MemberId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
