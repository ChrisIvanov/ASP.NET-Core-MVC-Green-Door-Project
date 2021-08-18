namespace GreenDoorProject.Test.Controllers
{
    using GreenDoorProject.Controllers;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Models.Books;
    using GreenDoorProject.Services.Books;
    using GreenDoorProject.Services.Books.Models;
    using GreenDoorProject.Services.Members;
    using GreenDoorProject.Services.Patrons;
    using GreenDoorProject.Test.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Collections.Generic;
    using Xunit;

    public class BooksControllerTest
    {
        [Fact]
        public void MethodDetailsShouldReturnViewResultWhenCalled()
        {
            //Arange
            var booksController = GetBooksController();

            var book = new BookServiceModel();

            //Act
            var result = booksController.Details(book);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void MethodReadBookShouldRetrunRedirectToActionResultWhenCalled()
        {
            //Arange
            var booksController = GetBooksController();

            using var data = DatabaseMock.Instance;

            var user = new Guest();

            var member = new Member
            {
                UserId = user.Id,
                MembershipId = 1
            };

            var book = CreateBook();

            var bookForReading = new BookServiceModel
            {
                BookTitle = book.BookTitle,
                AuthorFirstName = book.Author.FirstName,
                AuthorLastName = book.Author.LastName,
                Contents = book.Content
            };

            data.Users.Add(user);
            data.Members.Add(member);
            data.Books.Add(book);

            data.SaveChanges();

            //Act
            var result = booksController.ReadBook(bookForReading);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        private BooksController GetBooksController()
        {
            var result = new BooksController(null, Mock.Of<IBookService>(), Mock.Of<IPatronService>(), Mock.Of<IMemberService>());

            return result;
        }

        private Book CreateBook()
            => new Book
            {
                BookTitle = "Title",
                Description = "info",
                Author = GetAuthor(),
                Content = new byte[4],
                ImagePath = @"https://thumbs.dreamstime.com/z/ripe-small-pineapple-design-35901888.jpg",
                Pages = 4,
                Rating = 4,
                GenreId = 1,               
            };

        private Author GetAuthor()
            => new Author
            {
                FirstName = "Author",
                LastName = "Author",
                ImagePath = @"https://thumbs.dreamstime.com/z/ripe-small-pineapple-design-35901888.jpg",
                Details = "info",
                YearOfBirth = 123,
                YearOfDeath = 123,
                AuthorBooks = new List<Book>()
            };
    }
}
