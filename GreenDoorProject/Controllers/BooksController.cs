namespace GreenDoorProject.Controllers
{
    using System;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Books;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Services.Books;

    public class BooksController : Controller
    {
        private readonly IBookService books;
        private readonly GreenDoorProjectDbContext _data;

        public BooksController(
            GreenDoorProjectDbContext data, 
            IBookService books)
        {
            this._data = data;
            this.books = books;
        }

        [HttpGet]
        public IActionResult All([FromQuery]AllBooksQueryModel query)
        {
            if (!_data.Books.Any())
            {
                var error = "Currently there are no books in library.";

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

            var totalBooks = query.TotalBooks;

            query.Genres = bookGenres;
            query.Books = queryResult.Books;
            query.TotalBooks = totalBooks;

            return View(query);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add()
        {
            if (!this.UserIsAdmin())
            {
                return RedirectToAction(nameof(PatronController.BecomePatron), "Home");
            }

            return View(new AddBookFormModel
            {
                Genres = this.GetBookGenres()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddBookFormModel bookModel)
        {
            if (!this._data.Genres.Any(g => g.Id == bookModel.GenreId))
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
                        "The book you are trying to add is already in the database.");
            }

            var author = this._data
                .Authors
                .Where(a => a.FirstName == bookModel.AuthorFirstName &&
                        a.LastName == bookModel.AuthorLastName)
                .FirstOrDefault();

            var book = new Book
            {
                BookTitle = bookModel.BookTitle,
                Author = author,
                GenreId = bookModel.GenreId,
                ImagePath = bookModel.ImagePath,
                Pages = bookModel.Pages,
                Price = bookModel.Price,
                Description = bookModel.Description
            };

            this._data.Books.Add(book);
            this._data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult AddAuthor()
        {
            if (!this.UserIsAdmin())
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(new AddAuthorFormModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Roles]
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

            this._data.Authors.Add(author);
            this._data.SaveChanges();

            return RedirectToAction("Books", "Add");
        }

        public IActionResult Buy()
        {


            return RedirectToAction("Books", "Buy");
        }

        private bool ExistingBookCheck(AddBookFormModel bookModel)
            => this._data
                .Books
                .Any(b => b.BookTitle == bookModel.BookTitle);

        private bool ExistingAuthorCheck(AddBookFormModel bookModel)
            => this._data
                    .Authors
                    .Any(a => a.FirstName == bookModel.AuthorFirstName &&
                        a.LastName == bookModel.AuthorLastName);

        private bool UserIsAdmin()
            => this._data.Users
                .Any(a => a.Id == this.User.GetId());

        private IEnumerable<BookGenreViewModel> GetBookGenres()
            => this._data
                .Genres
                .Select(b => new BookGenreViewModel
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();
    }
}
