namespace GreenDoorProject.Models.Books
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Author;
    
    public class AddAuthorFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Year of Birth")]
        public int YearOfBirth { get; set; }

        [Display(Name = "Year of Death(If the author has passed away)")]
        public int YearOfDeath { get; set; }

        public string Details { get; set; }
    }
}
