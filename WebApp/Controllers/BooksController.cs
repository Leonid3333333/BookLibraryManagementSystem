using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLibraryManagementSystem.Data;
using BookLibraryManagementSystem.Models;

namespace BookLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet("GetBooks")]
        public ActionResult<IEnumerable<Book>> GetBooks() => _context.Books.Include(b => b.Authors).ToList();

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = _context.Books.Include(b => b.Authors).FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpPost("AddBook")]
        public ActionResult<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("EditBook")]
        public IActionResult EditBook(int id, Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedBook).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(b => b.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("DeleteBook")]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("{bookId}/issue/{memberId}")]
        public IActionResult IssueBook(int bookId, int memberId)
        {
            var book = _context.Books.Find(bookId);
            var member = _context.Members.Find(memberId);

            if (book == null || member == null || book.Status != BookStatus.Available)
            {
                return BadRequest();
            }

            book.Status = BookStatus.Issued;
            var transaction = new TransactionLog
            {
                ISBN = book.ISBN,
                MemberId = memberId,
                IssueDate = System.DateTime.Now
            };
            _context.TransactionLogs.Add(transaction);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("{bookId}/return/{memberId}")]
        public IActionResult ReturnBook(int bookId, int memberId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null || book.Status != BookStatus.Issued)
            {
                return BadRequest();
            }

            book.Status = BookStatus.Available;
            var transaction = _context.TransactionLogs.FirstOrDefault(t => t.ISBN == book.ISBN && t.MemberId == memberId && t.ReturnDate == null);
            if (transaction != null)
            {
                transaction.ReturnDate = System.DateTime.Now;
            }
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Book>> SearchBooks(string title, string authorName, BookCategory? category)
        {
            var query = _context.Books.Include(b => b.Authors).AsQueryable();

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
    }
}
