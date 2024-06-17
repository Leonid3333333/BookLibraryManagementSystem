using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using BookLibraryManagementSystem.Models;

namespace BookLibraryManagementSystem.Tests
{
    public class LibraryManagerTests
    {
        private readonly Mock<LibraryContext> _mockContext;
        private readonly Mock<DbSet<Book>> _mockBookSet;
        private readonly Mock<DbSet<Author>> _mockAuthorSet;
        private readonly Mock<DbSet<Member>> _mockMemberSet;
        private readonly Mock<DbSet<TransactionLog>> _mockTransactionLogSet;
        private readonly LibraryManager _libraryManager;

        public LibraryManagerTests()
        {
            _mockContext = new Mock<LibraryContext>();
            _mockBookSet = new Mock<DbSet<Book>>();
            _mockAuthorSet = new Mock<DbSet<Author>>();
            _mockMemberSet = new Mock<DbSet<Member>>();
            _mockTransactionLogSet = new Mock<DbSet<TransactionLog>>();

            _mockContext.Setup(m => m.Books).Returns(_mockBookSet.Object);
            _mockContext.Setup(m => m.Authors).Returns(_mockAuthorSet.Object);
            _mockContext.Setup(m => m.Members).Returns(_mockMemberSet.Object);
            _mockContext.Setup(m => m.TransactionLogs).Returns(_mockTransactionLogSet.Object);

            _libraryManager = new LibraryManager(_mockContext.Object);
        }

        [Fact]
        public void AddBook_ShouldAddBook()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "1234567890",
                Title = "Test Book",
                ReleaseYear = 2023,
                Category = BookCategory.Fiction,
                Status = BookStatus.Available
            };

            _mockBookSet.Setup(m => m.Add(It.IsAny<Book>())).Callback<Book>(b => book = b);

            // Act
            _libraryManager.AddBook(book);

            // Assert
            _mockBookSet.Verify(m => m.Add(It.Is<Book>(b => b == book)), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void EditBook_ShouldEditBook()
        {
            // Arrange
            var bookId = 1;
            var existingBook = new Book
            {
                Id = bookId,
                ISBN = "1234567890",
                Title = "Old Title",
                ReleaseYear = 2020,
                Category = BookCategory.Fiction,
                Status = BookStatus.Available
            };
            var updatedBook = new Book
            {
                ISBN = "0987654321",
                Title = "New Title",
                ReleaseYear = 2023,
                Category = BookCategory.Science,
                Status = BookStatus.Reserved
            };

            _mockBookSet.Setup(m => m.Find(bookId)).Returns(existingBook);

            // Act
            _libraryManager.EditBook(bookId, updatedBook);

            // Assert
            Assert.Equal("0987654321", existingBook.ISBN);
            Assert.Equal("New Title", existingBook.Title);
            Assert.Equal(2023, existingBook.ReleaseYear);
            Assert.Equal(BookCategory.Science, existingBook.Category);
            Assert.Equal(BookStatus.Reserved, existingBook.Status);

            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void IssueBook_ShouldIssueBook()
        {
            // Arrange
            var bookId = 1;
            var memberId = 1;
            var book = new Book
            {
                Id = bookId,
                ISBN = "1234567890",
                Status = BookStatus.Available
            };
            var member = new Member
            {
                Id = memberId
            };

            _mockBookSet.Setup(m => m.Find(bookId)).Returns(book);
            _mockMemberSet.Setup(m => m.Find(memberId)).Returns(member);
            _mockTransactionLogSet.Setup(m => m.Add(It.IsAny<TransactionLog>())).Verifiable();

            // Act
            _libraryManager.IssueBook(bookId, memberId);

            // Assert
            Assert.Equal(BookStatus.Issued, book.Status);
            _mockTransactionLogSet.Verify(m => m.Add(It.Is<TransactionLog>(t => t.ISBN == book.ISBN && t.MemberId == memberId && t.IssueDate != null)), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void ReturnBook_ShouldReturnBook()
        {
            // Arrange
            var bookId = 1;
            var memberId = 1;
            var book = new Book
            {
                Id = bookId,
                ISBN = "1234567890",
                Status = BookStatus.Issued
            };
            var transaction = new TransactionLog
            {
                ISBN = book.ISBN,
                MemberId = memberId,
                IssueDate = DateTime.Now.AddDays(-5)
            };

            _mockBookSet.Setup(m => m.Find(bookId)).Returns(book);
            _mockTransactionLogSet.Setup(m => m.FirstOrDefault(It.IsAny<Func<TransactionLog, bool>>())).Returns(transaction);

            // Act
            _libraryManager.ReturnBook(bookId, memberId);

            // Assert
            Assert.Equal(BookStatus.Available, book.Status);
            Assert.NotNull(transaction.ReturnDate);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void AddMember_ShouldAddMember()
        {
            // Arrange
            var member = new Member
            {
                Name = "John",
                Surname = "Doe",
                YearOfBirth = 1990,
                Address = "123 Main St",
                DateOfRegistration = DateTime.Now
            };

            _mockMemberSet.Setup(m => m.Add(It.IsAny<Member>())).Callback<Member>(m => member = m);

            // Act
            _libraryManager.AddMember(member);

            // Assert
            _mockMemberSet.Verify(m => m.Add(It.Is<Member>(m => m == member)), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void EditMember_ShouldEditMember()
        {
            // Arrange
            var memberId = 1;
            var existingMember = new Member
            {
                Id = memberId,
                Name = "John",
                Surname = "Doe",
                YearOfBirth = 1990,
                Address = "123 Main St",
                DateOfRegistration = DateTime.Now
            };
            var updatedMember = new Member
            {
                Name = "Jane",
                Surname = "Smith",
                YearOfBirth = 1985,
                Address = "456 Oak St",
                DateOfRegistration = DateTime.Now.AddYears(-1)
            };

            _mockMemberSet.Setup(m => m.Find(memberId)).Returns(existingMember);

            // Act
            _libraryManager.EditMember(memberId, updatedMember);

            // Assert
            Assert.Equal("Jane", existingMember.Name);
            Assert.Equal("Smith", existingMember.Surname);
            Assert.Equal(1985, existingMember.YearOfBirth);
            Assert.Equal("456 Oak St", existingMember.Address);
            Assert.Equal(updatedMember.DateOfRegistration, existingMember.DateOfRegistration);

            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void ListAllBooks_ShouldReturnAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", ISBN = "1111", ReleaseYear = 2020, Category = BookCategory.Fiction, Status = BookStatus.Available },
                new Book { Id = 2, Title = "Book 2", ISBN = "2222", ReleaseYear = 2021, Category = BookCategory.NonFiction, Status = BookStatus.Issued }
            }.AsQueryable();

            _mockBookSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.Provider);
            _mockBookSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.Expression);
            _mockBookSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.ElementType);
            _mockBookSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

            // Act
            var result = _libraryManager.ListAllBooks();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Book 1", result[0].Title);
            Assert.Equal("Book 2", result[1].Title);
        }
    }
}
