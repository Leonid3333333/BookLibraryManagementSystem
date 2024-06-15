using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryManagementSystem.Models
{
    public class Author
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; }
        public string Surname { get; set; }
        public int YearOfBirth { get; set; }
        public List<Book> Books { get; set; } = new List<Book>(); // Initialize Books
    }
}
