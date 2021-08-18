
namespace GreenDoorProject.Models.Cinema
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Actor;
    using static Data.DataConstants.Info;

    public class ActorFormModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "Year Of Birth")]
        [Range(YearOfBirthMinValue, YearOfBirthMaxValue)]
        public short YearOfBirth { get; set; }

        [Display(Name = "Year Of Death")]
        public short? YearOfDeath { get; set; }

        [StringLength(DefaultClassInfoMaxLength)]
        public string Details { get; set; }
    }
}
