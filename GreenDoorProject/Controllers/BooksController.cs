namespace GreenDoorProject.Controllers
{
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Models.Books;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class BooksController : Controller
    {
        private readonly GreenDoorProjectDbContext data;

        public BooksController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            if (!data.Books.Any())
            {
                return Redirect("/Views/Shared/Error");
            }

            var books = this.data
                .Books
                .Select(b => new AllBooksListingModel
                {
                    BookTitle = b.BookTitle,
                    AuthorName = new string($"{b.Author.FirstName} {b.Author.LastName}"),
                    ImagePath = b.ImagePath,
                    Genre = b.Genre,
                    Price = b.Price
                })
                .ToList();

            return View(books);
        }

        public IActionResult Add() => View(new AddBookFormModel
        {
            Genres = this.GetBookGenres()
        });

        [HttpPost]
        public IActionResult Add(AddBookFormModel bookModel)
        {
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
                this.ModelState.AddModelError(nameof(bookModel.AuthorLastName), " Author does not exist.");

                return RedirectToAction("AddAuthor");
            }

            if (ExistingBookCheck(bookModel))
            {
                this.ModelState
                    .AddModelError(
                        nameof(bookModel.BookTitle),
                        "The book you are trying to add is already in the database.");
            }

            var author = this.data
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

            this.data.Books.Add(book);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private bool ExistingBookCheck(AddBookFormModel bookModel)
            => this.data
                .Books
                .Any(b => b.BookTitle == bookModel.BookTitle);

        public IActionResult AddAuthor() => View(new AddAuthorFormModel());

        [HttpPost]
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

            return RedirectToAction("/Views/Books/Add");
        }

        private bool ExistingAuthorCheck(AddBookFormModel bookModel)
            => this.data
                    .Authors
                    .Any(a => a.FirstName == bookModel.AuthorFirstName &&
                        a.LastName == bookModel.AuthorLastName);

        private IEnumerable<BookGenreViewModel> GetBookGenres()
            => this.data
                .Genres
                .Select(b => new BookGenreViewModel
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();
    }
}
