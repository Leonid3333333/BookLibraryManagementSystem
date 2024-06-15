using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; } // Primary Key
        public string ISBN { get; set; }
        public string Title { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>(); // Initialize Authors
        public int ReleaseYear { get; set; }
        public BookCategory Category { get; set; }
        public BookStatus Status { get; set; }
    }
}

