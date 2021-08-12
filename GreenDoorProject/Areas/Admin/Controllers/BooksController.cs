namespace GreenDoorProject.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Collections.Generic;
    using GreenDoorProject.Controllers;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Books;
    using GreenDoorProject.Services.Books;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : AdminController
    {
        private readonly IBookService books;
        private readonly GreenDoorProjectDbContext data;

        public BooksController(
            GreenDoorProjectDbContext data,
            IBookService books)
        {
            this.data = data;
            this.books = books;
        }

        [HttpPost]
        public IActionResult Add()
        {
            if (!this.User.IsAdmin())
            {
                return RedirectToAction("Home", "Index");
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
                Description = bookModel.Description
            };

            this.data.Books.Add(book);
            this.data.SaveChanges();

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
                YearOfDeath = authorModel.YearOfDeath,
                Details = authorModel.Details
            };

            this.data.Authors.Add(author);
            this.data.SaveChanges();

            return RedirectToAction("Books", "Add");
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

        private bool UserIsAdmin()
            => this.data.Users
                .Any(a => a.Id == this.User.GetId());

        private IEnumerable<BookGenreViewModel> GetBookGenres()
            => this.data.Genres
                .Select(b => new BookGenreViewModel
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();
    }
}
