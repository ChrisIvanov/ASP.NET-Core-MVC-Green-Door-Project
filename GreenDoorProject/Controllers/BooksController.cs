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
        private readonly GreenDoorProjectDbContext data;

        public BooksController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        public IActionResult All(string brand, string searchTerm)
        {
            if (!data.Books.Any())
            {
                return Redirect("/Views/Shared/Error");
            }

            var booksQuery = this.data.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Genre.Name == brand);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                booksQuery = booksQuery.Where(b => 
                    b.BookTitle.ToLower().Contains(searchTerm.ToLower())
                    || (b.Author.FirstName + " " + b.Author.LastName).Contains(searchTerm.ToLower())
                    || b.Description.Contains(searchTerm.ToLower())
                    || b.Genre.Name.Contains(searchTerm.ToLower()));
            }

            var books = booksQuery
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

            var bookGenres = this.data
                .Books
                .Select(b => b.Genre.Name)
                .Distinct()
                .ToList();

            return View(new AllBooksQueryModel
            {
                Genres = bookGenres,
                Books = books,
                SearchTerm = searchTerm
            });
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.UserIsAdmin())
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
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
            => this.data.Admins
                .Any(a => a.UserId == this.User.GetId());

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
