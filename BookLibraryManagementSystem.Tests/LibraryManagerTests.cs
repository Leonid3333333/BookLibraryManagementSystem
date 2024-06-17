using Xunit;
using Moq;
using BookLibraryManagementSystem;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using BookLibraryManagementSystem.Models;

namespace BookLibraryManagementSystem.Tests
{
    public class LibraryManagerTests
    {
        private DbContextOptions<LibraryContext> GetInMemoryOptions() => new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "LibraryTestDatabase")
                .Options;

        [Fact]
        public void AddBook_ShouldAddBookToDatabase()
        {
            // Arrange
            var options = GetInMemoryOptions();

            using (var context = new LibraryContext(options))
            {
                var manager = new LibraryManager(context);
                var book = new Book
                {
                    ISBN = "12345",
                    Title = "Test Book",
                    ReleaseYear = 2020,
                    Category = BookCategory.Fiction,
                    Status = BookStatus.Available,
                    Authors = new List<Author>()
                };

                // Act
                manager.AddBook(book);
            }

            // Assert
            using (var context = new LibraryContext(options))
            {
                Assert.Equal(1, context.Books.Count());
                var book = context.Books.Include(b => b.Authors).FirstOrDefault();
                Assert.NotNull(book);
                Assert.Equal("12345", book.ISBN);
                Assert.Equal("Test Book", book.Title);
                Assert.Equal(2020, book.ReleaseYear);
                Assert.Equal(BookCategory.Fiction, book.Category);
                Assert.Equal(BookStatus.Available, book.Status);
            }
        }

        [Fact]
        public void IssueBook_ShouldUpdateBookStatusAndAddTransactionLog()
        {
            // Arrange
            var options = GetInMemoryOptions();

            using (var context = new LibraryContext(options))
            {
                var manager = new LibraryManager(context);

                var book = new Book
                {
                    ISBN = "12345",
                    Title = "Test Book",
                    ReleaseYear = 2020,
                    Category = BookCategory.Fiction,
                    Status = BookStatus.Available,
                    Authors = new List<Author>()
                };
                context.Books.Add(book);

                var member = new Member
                {
                    Name = "John",
                    Surname = "Doe",
                    YearOfBirth = 1985,
                    Address = "123 Main St",
                    DateOfRegistration = System.DateTime.Now
                };
                context.Members.Add(member);
                context.SaveChanges();

                // Act
                manager.IssueBook(book.Id, member.Id);
            }

            // Assert
            using (var context = new LibraryContext(options))
            {
                var book = context.Books.FirstOrDefault();
                Assert.NotNull(book);
                Assert.Equal(BookStatus.Issued, book.Status);

                var transaction = context.TransactionLogs.FirstOrDefault();
                Assert.NotNull(transaction);
                Assert.Equal("12345", transaction.ISBN);
                Assert.Equal(context.Members.First().Id, transaction.MemberId);
                Assert.NotNull(transaction.IssueDate);
                Assert.Null(transaction.ReturnDate);
            }
        }

        [Fact]
        public void ReturnBook_ShouldUpdateBookStatusAndSetReturnDate()
        {
            // Arrange
            var options = GetInMemoryOptions();

            using (var context = new LibraryContext(options))
            {
                var manager = new LibraryManager(context);

                var book = new Book
                {
                    ISBN = "12345",
                    Title = "Test Book",
                    ReleaseYear = 2020,
                    Category = BookCategory.Fiction,
                    Status = BookStatus.Available,
                    Authors = new List<Author>()
                };
                context.Books.Add(book);

                var member = new Member
                {
                    Name = "John",
                    Surname = "Doe",
                    YearOfBirth = 1985,
                    Address = "123 Main St",
                    DateOfRegistration = System.DateTime.Now
                };
                context.Members.Add(member);
                context.SaveChanges();

                manager.IssueBook(book.Id, member.Id);

                // Act
                manager.ReturnBook(book.Id, member.Id);
            }

            // Assert
            using (var context = new LibraryContext(options))
            {
                var book = context.Books.FirstOrDefault();
                Assert.NotNull(book);
                Assert.Equal(BookStatus.Available, book.Status);

                var transaction = context.TransactionLogs.FirstOrDefault();
                Assert.NotNull(transaction);
                Assert.Equal("12345", transaction.ISBN);
                Assert.Equal(context.Members.First().Id, transaction.MemberId);
                Assert.NotNull(transaction.IssueDate);
                Assert.NotNull(transaction.ReturnDate);
            }
        }
    }
}
