using System.Collections.Generic;

namespace BookLibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();
        public int ReleaseYear { get; set; }
        public BookCategory Category { get; set; }
        public BookStatus Status { get; set; }
    }
}
