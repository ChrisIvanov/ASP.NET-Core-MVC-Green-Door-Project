namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Author
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int YearOfBirth { get; set; }

        public int? YearOfDeath { get; set; }

        public string Details { get; set; }

        public ICollection<Book> AuthorBooks { get; set; } = new List<Book>();
    }
}