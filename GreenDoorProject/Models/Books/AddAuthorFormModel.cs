namespace GreenDoorProject.Models.Books
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Author;
    
    public class AddAuthorFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

        [Required]
        public int YearOfBirth { get; set; }

        public int? YearOfDeath { get; set; }

        public string Details { get; set; }
    }
}
