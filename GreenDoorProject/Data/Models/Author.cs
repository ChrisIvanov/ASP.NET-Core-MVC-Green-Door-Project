namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Author
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime YearOfBirth { get; set; }

        public DateTime? YearOfDeath { get; set; }

        public string Details { get; set; }

        public ICollection<Book> AuthorBooks { get; set; } = new List<Book>();
    }
}