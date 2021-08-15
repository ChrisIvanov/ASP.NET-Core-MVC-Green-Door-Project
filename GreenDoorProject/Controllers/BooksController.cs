namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Books;
    using GreenDoorProject.Services.Books;
    using GreenDoorProject.Services.Books.Models;
    using GreenDoorProject.Services.Members;
    using GreenDoorProject.Services.Patrons;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : Controller
    {
        private readonly IBookService books;
        private readonly IPatronService patrons;
        private readonly IMemberService members;
        private readonly GreenDoorProjectDbContext data;

        public BooksController(
            GreenDoorProjectDbContext data,
            IBookService books,
            IPatronService patrons,
            IMemberService members)
        {
            this.data = data;
            this.books = books;
            this.patrons = patrons;
            this.members = members;
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
        public IActionResult Details(BookServiceModel book)
        {
            var bookDetails = this.books.Details(book.Id);

            return View(bookDetails);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ReadBook(string bookId)
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

            var userId = User.GetId();

            if (members.IsMember(userId))
            {
                return View(book);
            }
            else
            {
                if (patrons.IsPatron(userId))
                {
                    if (this.patrons.GetTokens(userId) < 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    this.patrons.UseToken(userId);

                    return View(book);
                }
            }

            return RedirectToAction("BecomeMember", "Member");
        }
    }
}
