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

    using static AdminConstants;
    using GreenDoorProject.Services.Books.Models;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class BooksController : Controller
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

        public IActionResult AdminAll([FromQuery] AllBooksQueryModel query) 
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
                AllBooksQueryModel.BooksPerPage,
                query.ShowOnlyAuthors);

            var bookGenres = this.books.AllBookGenres();

            var totalBooks = queryResult.TotalBooks;

            query.Genres = bookGenres;
            query.Books = queryResult.Books;
            query.TotalBooks = totalBooks;
            query.ShowOnlyAuthors = queryResult.ShowOnlyAuthors;

            return View(query);
        }

        public IActionResult AdminAllAuthors()
        {
            if (!data.Authors.Any())
            {
                return RedirectToAction("AdminAll", "Books");
            }

            var authors = this.data.Authors.ToList();

            var retrunAuthors = new List<AuthorDetailsViewModel>();

            foreach (var author in authors)
            {
                retrunAuthors.Add(new AuthorDetailsViewModel
                {
                    Id = author.Id,
                    FullName = author.FirstName + " " + author.LastName,
                    ImagePath = author.ImagePath,
                    YearOfBirth = author.YearOfBirth,
                    YearOfDeath = author.YearOfDeath,
                    Details = author.Details
                });
            }
            

            return View(retrunAuthors);
        }

        [HttpGet]
        [Authorize]
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

            bookModel.Content = fileInBytes;

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

            return RedirectToAction("AdminAll", "Books");
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
                ImagePath = authorModel.ImagePath,
                YearOfBirth = authorModel.YearOfBirth,
                YearOfDeath = authorModel.YearOfDeath,
                Details = authorModel.Details
            };

            this.data.Authors.Add(author);
            this.data.SaveChanges();

            return RedirectToAction("AdminAll", "Books");
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            if (!User.IsAdmin())
            {
                ModelState.AddModelError
                    (string.Empty, "You are not authorized to make changes to the website content.");

                return RedirectToAction("AdminAll", "Books");
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
                Content = book.Contents
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

            return RedirectToAction("AdminAll", "Books");
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditAuthor(AuthorFormModel model, string id)
        {

            var author = this.data.Authors
                .Where(b => b.Id == id)
                .FirstOrDefault();

            var edited = this.books.EditAuhtor(
                id,
                author.FirstName,
                author.LastName,
                author.ImagePath,
                author.YearOfBirth,
                author.YearOfDeath,
                author.Details);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminAllAuthors", "Books");
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

            return View("AdminAll", "Books");
        }

        public IActionResult DeleteAuthor(string id)
        {
            var author = this.data.Authors.Find(id);

            if (author == null)
            {
                this.ModelState.AddModelError(nameof(BooksController), "There is no author with this Id in the database.");
            }
            else
            {
                var books = this.data.Books
                    .Where(b => b.Author == author)
                    .ToList();

                if (books.Any())
                {
                    foreach (var book in books)
                    {
                        this.data.Books.Remove(book);
                    }
                }

                this.data.Authors.Remove(author);             
                this.data.SaveChanges();
            }

            return View("AdminAllAuthors", "Books");
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
