using BookLibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibraryManagementSystem
{
    public class LibraryManager
    {
        private readonly LibraryContext _context;

        public LibraryManager()
        {
            _context = new LibraryContext();
        }

        public void AddBook(Book book)
        {
            if (book.Authors == null)
            {
                book.Authors = new List<Author>();
            }

            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void EditBook(int id, Book updatedBook)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                book.ISBN = updatedBook.ISBN;
                book.Title = updatedBook.Title;
                book.ReleaseYear = updatedBook.ReleaseYear;
                book.Category = updatedBook.Category;
                book.Status = updatedBook.Status;
                book.Authors = updatedBook.Authors;
                _context.SaveChanges();
            }
        }

        public void IssueBook(int bookId, int memberId)
        {
            var book = _context.Books.Find(bookId);
            var member = _context.Members.Find(memberId);

            if (book != null && member != null && book.Status == BookStatus.Available)
            {
                book.Status = BookStatus.Issued;
                var transaction = new TransactionLog
                {
                    ISBN = book.ISBN,
                    MemberId = memberId,
                    IssueDate = DateTime.Now
                };
                _context.TransactionLogs.Add(transaction);
                _context.SaveChanges();
            }
        }

        public void ReturnBook(int bookId, int memberId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null && book.Status == BookStatus.Issued)
            {
                book.Status = BookStatus.Available;
                var transaction = _context.TransactionLogs.FirstOrDefault(t => t.ISBN == book.ISBN && t.MemberId == memberId && t.ReturnDate == null);
                if (transaction != null)
                {
                    transaction.ReturnDate = DateTime.Now;
                }
                _context.SaveChanges();
            }
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public void EditMember(int id, Member updatedMember)
        {
            var member = _context.Members.Find(id);
            if (member != null)
            {
                member.Name = updatedMember.Name;
                member.Surname = updatedMember.Surname;
                member.YearOfBirth = updatedMember.YearOfBirth;
                member.Address = updatedMember.Address;
                member.DateOfRegistration = updatedMember.DateOfRegistration;
                _context.SaveChanges();
            }
        }

        public List<Book> SearchBooks(string title = null, string authorName = null, BookCategory? category = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(authorName))
            {
                query = query.Where(b => b.Authors.Any(a => a.Name.Contains(authorName) || a.Surname.Contains(authorName)));
            }

            if (category.HasValue)
            {
                query = query.Where(b => b.Category == category.Value);
            }

            return query.ToList();
        }

        public List<TransactionLog> GetIssueReturnHistory()
        {
            return _context.TransactionLogs.ToList();
        }

        public List<Book> ListAllBooks()
        {
            return _context.Books.Include(b => b.Authors).ToList();
        }

        public Author GetAuthorById(int id)
        {
            return _context.Authors.Find(id);
        }
    }
}