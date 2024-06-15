using Microsoft.EntityFrameworkCore;
using BookLibraryManagementSystem.Models;

namespace BookLibraryManagementSystem.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
    }
}
