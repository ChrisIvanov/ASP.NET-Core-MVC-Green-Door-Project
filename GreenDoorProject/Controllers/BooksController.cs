namespace GreenDoorProject.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Books;
    using GreenDoorProject.Services.Books;
    using GreenDoorProject.Services.Books.Models;
    using GreenDoorProject.Services.Patrons;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using static Data.DataConstants.Book;

    public class BooksController : Controller
    {
        private readonly IBookService books;
        private readonly IPatronService patrons;
        private readonly GreenDoorProjectDbContext data;

        public BooksController(
            GreenDoorProjectDbContext data,
            IBookService books)
        {
            this.data = data;
            this.books = books;
        }

        [HttpGet]
        [Authorize]
        public IActionResult All([FromQuery] AllBooksQueryModel query)
        {
            if (!data.Books.Any())
            {
                var error = "Currently there are no books in the library.";

                var errorModel = new AllBooksQueryModel
                {
                    ModelError = error
                };

                return View(errorModel);
            }

            var queryResult = this.books.All(
                query.Genre,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllBooksQueryModel.BooksPerPage);

            var bookGenres = this.books.AllBookGenres();

            var totalBooks = queryResult.TotalBooks;

            query.Genres = bookGenres;
            query.Books = queryResult.Books;
            query.TotalBooks = totalBooks;

            return View(query);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return View(new AddBookFormModel
            {
                Genres = this.GetBookGenres()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddBookFormModel bookModel, IFormFile file)
        {
            var fileInMemory = new MemoryStream();
            file.CopyTo(fileInMemory);
            var fileInBytes = fileInMemory.ToArray();

            if (!this.data.Genres.Any(g => g.Id == bookModel.GenreId))
            {
                this.ModelState.AddModelError(nameof(bookModel.GenreId), "Genre does not exist.");
            }

            if (!ModelState.IsValid)
            {
                bookModel.Genres = this.GetBookGenres();

                return View(bookModel);
            }

            if (!ExistingAuthorCheck(bookModel))
            {
                this.ModelState.AddModelError(nameof(bookModel.AuthorLastName), "Author does not exist.");

                return RedirectToAction("AddAuthor");
            }

            if (ExistingBookCheck(bookModel))
            {
                this.ModelState
                    .AddModelError(
                        nameof(bookModel.BookTitle),
                        "The book already exists.");
            }

            var authorId = books.GetAuthorId(
                bookModel.AuthorFirstName,
                bookModel.AuthorLastName);

            var author = books.GetAuthor(authorId);

            var book = new Book
            {
                BookTitle = bookModel.BookTitle,
                GenreId = bookModel.GenreId,
                ImagePath = bookModel.ImagePath,
                Pages = bookModel.Pages,
                Description = bookModel.Description,
                Content = fileInBytes,
                AuthorId = authorId
            };

            this.data.Books.Add(book);
            author.AuthorBooks.Add(book);

            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult AddAuthor()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Index");
            }

            return View(new AddAuthorFormModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAuthor(AddAuthorFormModel authorModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(authorModel);
            }

            var author = new Author
            {
                FirstName = authorModel.FirstName,
                LastName = authorModel.LastName,
                YearOfBirth = authorModel.YearOfBirth,
                YearOfDeath = authorModel.YearOfDeath == null ? null : authorModel.YearOfDeath,
                Details = authorModel.Details
            };

            this.data.Authors.Add(author);
            this.data.SaveChanges();

            return RedirectToAction("Add", "Books");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(BookServiceModel book)
        {
            var bookDetails = this.books.Details(book.Id);

            return View(bookDetails);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Vote(BookServiceModel model)
        {
            

            return RedirectToAction("Details", "Books");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ReadAsMember(string bookId)
        {
            var books = this.data.Books.AsQueryable();

            var book = books
                .Where(b => b.Id == bookId)
                .Select(b => new BookContentsViewModel
                {
                    Title = b.BookTitle,
                    Author = b.Author.FirstName + " " + b.Author.LastName,
                    Contents = b.Content
                })
                .FirstOrDefault();

            return View(book);
        }
        private IEnumerable<BookGenreViewModel> GetBookGenres()
            => this.data.Genres
                .Select(b => new BookGenreViewModel
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();

        [HttpGet]
        [Authorize]
        public IActionResult ReadAsPatron(string bookId)
        {
            var userId = User.GetId();

            if (this.patrons.GetTokens(userId) < 1)
            {
                return RedirectToAction("Home", "Index");
            }

            this.patrons.UseToken(User.GetId());

            var books = this.data.Books.AsQueryable();

            var book = books
                .Where(b => b.Id == bookId)
                .Select(b => new BookContentsViewModel
                {
                    Title = b.BookTitle,
                    Author = b.Author.FirstName + " " + b.Author.LastName,
                    Contents = b.Content
                })
                .FirstOrDefault();

            return View(book);
        }

        private bool ExistingBookCheck(AddBookFormModel bookModel)
            => this.data
                .Books
                .Any(b => b.BookTitle == bookModel.BookTitle);

        private bool ExistingAuthorCheck(AddBookFormModel bookModel)
            => this.data
                    .Authors
                    .Any(a => a.FirstName == bookModel.AuthorFirstName &&
                        a.LastName == bookModel.AuthorLastName);
    }
}
