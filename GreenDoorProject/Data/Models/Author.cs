﻿namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Author;
    using static DataConstants.Info;

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
        [Url]
        public string ImagePath { get; set; }

        [Required]
        public int YearOfBirth { get; set; }

        public int YearOfDeath { get; set; }

        [StringLength(DefaultClassInfoMaxLength)]
        public string Details { get; set; }

        public ICollection<Book> AuthorBooks { get; set; } = new List<Book>();
    }
}