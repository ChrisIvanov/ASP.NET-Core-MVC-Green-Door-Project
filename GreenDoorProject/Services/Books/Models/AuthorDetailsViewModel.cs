namespace GreenDoorProject.Services.Books.Models
{
    using System.Collections.Generic;
    using GreenDoorProject.Data.Models;

    public class AuthorDetailsViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string ImagePath { get; set; }

        public int YearOfBirth { get; set; }

        public int? YearOfDeath { get; set; }

        public string Details { get; set; }

        public ICollection<Book> AuthorBooks { get; set; }
    }
}
