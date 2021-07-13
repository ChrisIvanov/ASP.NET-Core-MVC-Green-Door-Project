namespace GreenDoorProject.Models.Books
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class AddAuthorFormModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime YearOfBirth { get; set; }

        public DateTime? YearOfDeath { get; set; }

        public string Details { get; set; }
    }
}
