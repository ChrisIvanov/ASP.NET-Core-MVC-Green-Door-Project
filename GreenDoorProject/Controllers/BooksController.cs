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

    public class BooksController : Controller
    {
        private readonly GreenDoorProjectDbContext _data;

        public BooksController(GreenDoorProjectDbContext data)
        {
            this._data = data;
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

            var booksQuery = this._data.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Genre))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Genre.Name == query.Genre);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                booksQuery = booksQuery.Where(b => 
                    b.BookTitle.ToLower().Contains(query.SearchTerm.ToLower())
                    || (b.Author.FirstName + " " + b.Author.LastName).Contains(query.SearchTerm.ToLower())
                    || b.Description.Contains(query.SearchTerm.ToLower())
                    || b.Genre.Name.Contains(query.SearchTerm.ToLower()));
            }

            var totalBooks = booksQuery.Count();

            var books = booksQuery
                .Skip((query.CurrentPage - 1) * AllBooksQueryModel.BooksPerPage)
                .Take(AllBooksQueryModel.BooksPerPage)
                .OrderBy(a => a.Id)
                .Select(b => new BookListingViewModel
                {
                    BookTitle = b.BookTitle,
                    AuthorName = new string($"{b.Author.FirstName} {b.Author.LastName}"),
                    ImagePath = b.ImagePath,
                    Genre = b.Genre,
                    Price = b.Price
                })
                .ToList();

            booksQuery = query.Sorting switch
            {
                BookSorting.BookTitleAscending => booksQuery.OrderBy(b => b.BookTitle),
                BookSorting.BookTitleDescending => booksQuery.OrderByDescending(b => b.BookTitle),
                BookSorting.AuthorName => booksQuery.OrderBy(b => b.Author.LastName ).ThenBy(b => b.Author.FirstName),
                BookSorting.Recommendation => booksQuery.OrderByDescending(b => b.Recomendations),
                BookSorting.PriceAscending => booksQuery.OrderBy(b => b.Price),
                BookSorting.PriceDescending => booksQuery.OrderByDescending(b => b.Price),
                _ => booksQuery.OrderByDescending(b => b.Id)
            };

            var bookGenres = this._data
                .Books
                .Select(b => b.Genre.Name)
                .Distinct()
                .ToList();

            query.Books = books;
            query.Genres = bookGenres;
            query.TotalBooks = totalBooks;

            return View(query);
        }

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
            => this._data.Admins
                .Any(a => a.UserId == this.User.GetId());

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
