﻿namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Author;
    using static Data.DataConstants.Info;

    public class Author
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

        [Required]
        public int YearOfBirth { get; set; }

        public int? YearOfDeath { get; set; }

        [StringLength(DefaultClassInfoMaxLength)]
        public string Details { get; set; }

        [Required]
        public string AdminId { get; init; }
        public Admin Admin { get; init; }

        public ICollection<Book> AuthorBooks { get; set; } = new List<Book>();
    }
}