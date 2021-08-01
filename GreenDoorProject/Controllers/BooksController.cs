namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Books;
    using GreenDoorProject.Services.Books;
    using GreenDoorProject.Services.Patrons;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "User, Member, Admin")]
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

            var totalBooks = query.TotalBooks;

            query.Genres = bookGenres;
            query.Books = queryResult.Books;
            query.TotalBooks = totalBooks;

            return View(query);
        }

        [HttpGet]
        [Authorize(Roles = "User, Member, Admin")]
        public IActionResult Details(string bookId)
        {
            var book = this.books.Details(bookId);

            return View(book);
        }

        [HttpGet]
        [Authorize(Roles = "Member, Admin")]
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

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
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
    }
}
