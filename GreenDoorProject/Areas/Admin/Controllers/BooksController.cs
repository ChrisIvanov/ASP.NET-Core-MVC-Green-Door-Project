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
    using System.IO;
    using Microsoft.AspNetCore.Http;

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

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            return View(new BookFormModel
            {
                Genres = this.GetBookGenres()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(BookFormModel bookModel, IFormFile file)
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

            return View(new AuthorFormModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAuthor(AuthorFormModel authorModel)
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

            return RedirectToAction("Add", "Books");
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            if (!User.IsAdmin())
            {
                ModelState.AddModelError
                    (string.Empty, "You are not authorized to make changes to the website content.");

                return RedirectToAction("All", "Cinema");
            }

            var book = this.books.Details(id);

            return View(new BookFormModel
            {
                BookTitle = book.BookTitle,
                AuthorFirstName = book.AuthorFirstName,
                AuthorLastName = book.AuthorLastName,
                GenreId = book.Genre.Id,
                ImagePath = book.ImagePath,
                Pages = book.Pages,
                Rating = book.Rating,
                Description = book.Description,
                Content = book.Content
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(BookFormModel model, string id)
        {

            var book = this.data.Books
                .Where(b => b.Id == id)
                .FirstOrDefault();

            var edited = this.books.Edit(
                id,
                book.BookTitle,
                book.Author.FirstName,
                book.Author.LastName,
                book.ImagePath,
                book.Pages,
                book.Rating,
                book.Description,
                book.Content);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction("All", "Books");
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var book = this.data.Books.Find(id);

            if (book == null)
            {
                this.ModelState.AddModelError(nameof(BooksController), "There is no book with this Id in the database.");
            }
            else
            {
                var authorsBooks = this.data.Authors
                    .Where(b => b.AuthorBooks.Contains(book))
                    .Select(b => b.AuthorBooks)
                    .ToList();

                if (authorsBooks.Any())
                {
                    foreach (var authorBook in authorsBooks)
                    {
                        authorsBooks.Remove(authorBook);
                    }
                }

                this.data.Books.Remove(book);
                this.data.SaveChanges();
            }

            return View("All", "Cinema");
        }

        private bool ExistingBookCheck(BookFormModel bookModel)
            => this.data
                .Books
                .Any(b => b.BookTitle == bookModel.BookTitle);

        private bool ExistingAuthorCheck(BookFormModel bookModel)
            => this.data
                    .Authors
                    .Any(a => a.FirstName == bookModel.AuthorFirstName &&
                        a.LastName == bookModel.AuthorLastName);

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
