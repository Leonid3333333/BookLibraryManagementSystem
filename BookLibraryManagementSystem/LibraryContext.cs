using BookLibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookLibraryManagementSystem
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
                .UseSqlServer(@"Server=localhost;Database=BookLibrary;Trusted_Connection=True;TrustServerCertificate=True;")
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id); // Primary Key for Book

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity(j => j.ToTable("BookAuthors"));

            modelBuilder.Entity<Author>()
                .HasKey(a => a.Id); // Primary Key for Author

            modelBuilder.Entity<Member>()
                .HasKey(m => m.Id); // Primary Key for Member

            modelBuilder.Entity<TransactionLog>()
                .HasKey(t => t.Id); // Primary Key for TransactionLog

            modelBuilder.Entity<TransactionLog>()
                .Property(t => t.ReturnDate)
                .IsRequired(false);
        }
    }
}
