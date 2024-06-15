using BookLibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibraryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var libraryManager = new LibraryManager();

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Edit Book");
                Console.WriteLine("3. Issue Book");
                Console.WriteLine("4. Return Book");
                Console.WriteLine("5. Add Member");
                Console.WriteLine("6. Edit Member");
                Console.WriteLine("7. Search Books");
                Console.WriteLine("8. View Issue/Return History");
                Console.WriteLine("9. List All Books");
                Console.WriteLine("10. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBook(libraryManager);
                        break;
                    case "2":
                        EditBook(libraryManager);
                        break;
                    case "3":
                        IssueBook(libraryManager);
                        break;
                    case "4":
                        ReturnBook(libraryManager);
                        break;
                    case "5":
                        AddMember(libraryManager);
                        break;
                    case "6":
                        EditMember(libraryManager);
                        break;
                    case "7":
                        SearchBooks(libraryManager);
                        break;
                    case "8":
                        ViewIssueReturnHistory(libraryManager);
                        break;
                    case "9":
                        ListAllBooks(libraryManager);
                        break;
                    case "10":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddBook(LibraryManager libraryManager)
        {
            Console.Write("Enter ISBN: ");
            string isbn = Console.ReadLine();

            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Release Year: ");
            int releaseYear = int.Parse(Console.ReadLine());

            Console.Write("Enter Category (0: Fiction, 1: NonFiction, 2: Mystery, 3: Fantasy, 4: Biography, 5: Science, 6: History): ");
            BookCategory category = (BookCategory)int.Parse(Console.ReadLine());

            Console.Write("Enter Status (0: Available, 1: Issued, 2: Reserved): ");
            BookStatus status = (BookStatus)int.Parse(Console.ReadLine());

            var book = new Book
            {
                ISBN = isbn,
                Title = title,
                ReleaseYear = releaseYear,
                Category = category,
                Status = status,
                Authors = new List<Author>()
            };

            // Add authors
            Console.Write("How many authors? ");
            int authorCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < authorCount; i++)
            {
                Console.Write("Enter Author Id: ");
                int authorId = int.Parse(Console.ReadLine());

                // Fetch author by ID
                var author = libraryManager.GetAuthorById(authorId);
                if (author != null)
                {
                    book.Authors.Add(author);
                }
                else
                {
                    Console.WriteLine($"Author with Id {authorId} not found.");
                }
            }

            libraryManager.AddBook(book);
            Console.WriteLine("Book added successfully.");
        }

        static void EditBook(LibraryManager libraryManager)
        {
            Console.Write("Enter Book Id: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter ISBN: ");
            string isbn = Console.ReadLine();

            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Release Year: ");
            int releaseYear = int.Parse(Console.ReadLine());

            Console.Write("Enter Category (0: Fiction, 1: NonFiction, 2: Mystery, 3: Fantasy, 4: Biography, 5: Science, 6: History): ");
            BookCategory category = (BookCategory)int.Parse(Console.ReadLine());

            Console.Write("Enter Status (0: Available, 1: Issued, 2: Reserved): ");
            BookStatus status = (BookStatus)int.Parse(Console.ReadLine());

            var book = new Book
            {
                ISBN = isbn,
                Title = title,
                ReleaseYear = releaseYear,
                Category = category,
                Status = status,
                Authors = new List<Author>()
            };

            // Add authors
            Console.Write("How many authors? ");
            int authorCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < authorCount; i++)
            {
                Console.Write("Enter Author Id: ");
                int authorId = int.Parse(Console.ReadLine());

                // Fetch author by ID
                var author = libraryManager.GetAuthorById(authorId);
                if (author != null)
                {
                    book.Authors.Add(author);
                }
                else
                {
                    Console.WriteLine($"Author with Id {authorId} not found.");
                }
            }

            libraryManager.EditBook(id, book);
            Console.WriteLine("Book edited successfully.");
        }

        static void IssueBook(LibraryManager libraryManager)
        {
            Console.Write("Enter Book Id: ");
            int bookId = int.Parse(Console.ReadLine());

            Console.Write("Enter Member Id: ");
            int memberId = int.Parse(Console.ReadLine());

            libraryManager.IssueBook(bookId, memberId);
            Console.WriteLine("Book issued successfully.");
        }

        static void ReturnBook(LibraryManager libraryManager)
        {
            Console.Write("Enter Book Id: ");
            int bookId = int.Parse(Console.ReadLine());

            Console.Write("Enter Member Id: ");
            int memberId = int.Parse(Console.ReadLine());

            libraryManager.ReturnBook(bookId, memberId);
            Console.WriteLine("Book returned successfully.");
        }

        static void AddMember(LibraryManager libraryManager)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Surname: ");
            string surname = Console.ReadLine();

            Console.Write("Enter Year of Birth: ");
            int yearOfBirth = int.Parse(Console.ReadLine());

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            var member = new Member
            {
                Name = name,
                Surname = surname,
                YearOfBirth = yearOfBirth,
                Address = address,
                DateOfRegistration = DateTime.Now
            };

            libraryManager.AddMember(member);
            Console.WriteLine("Member added successfully.");
        }

        static void EditMember(LibraryManager libraryManager)
        {
            Console.Write("Enter Member Id: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Surname: ");
            string surname = Console.ReadLine();

            Console.Write("Enter Year of Birth: ");
            int yearOfBirth = int.Parse(Console.ReadLine());

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            var member = new Member
            {
                Name = name,
                Surname = surname,
                YearOfBirth = yearOfBirth,
                Address = address,
                DateOfRegistration = DateTime.Now
            };

            libraryManager.EditMember(id, member);
            Console.WriteLine("Member edited successfully.");
        }

        static void SearchBooks(LibraryManager libraryManager)
        {
            Console.Write("Enter Title (optional): ");
            string title = Console.ReadLine();

            Console.Write("Enter Author Name (optional): ");
            string authorName = Console.ReadLine();

            Console.Write("Enter Category (optional, 0: Fiction, 1: NonFiction, 2: Mystery, 3: Fantasy, 4: Biography, 5: Science, 6: History): ");
            string categoryInput = Console.ReadLine();
            BookCategory? category = null;
            if (!string.IsNullOrEmpty(categoryInput))
            {
                category = (BookCategory)int.Parse(categoryInput);
            }

            var books = libraryManager.SearchBooks(title, authorName, category);

            foreach (var book in books)
            {
                Console.WriteLine($"Id: {book.Id}, Title: {book.Title}, ISBN: {book.ISBN}, Year: {book.ReleaseYear}, Category: {book.Category}, Status: {book.Status}");
            }
        }

        static void ViewIssueReturnHistory(LibraryManager libraryManager)
        {
            var history = libraryManager.GetIssueReturnHistory();

            foreach (var log in history)
            {
                Console.WriteLine($"ISBN: {log.ISBN}, Member Id: {log.MemberId}, Issue Date: {log.IssueDate}, Return Date: {log.ReturnDate}");
            }
        }

        static void ListAllBooks(LibraryManager libraryManager)
        {
            var books = libraryManager.ListAllBooks();

            foreach (var book in books)
            {
                Console.WriteLine($"Id: {book.Id}, Title: {book.Title}, ISBN: {book.ISBN}, Year: {book.ReleaseYear}, Category: {book.Category}, Status: {book.Status}, Authors: {string.Join(", ", book.Authors.Select(a => $"{a.Name} {a.Surname}"))}");
            }
        }
    }
}
