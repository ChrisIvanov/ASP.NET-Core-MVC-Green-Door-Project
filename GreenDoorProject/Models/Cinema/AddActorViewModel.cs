
namespace GreenDoorProject.Models.Cinema
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Actor;
    using static Data.DataConstants.Info;

    public class AddActorViewModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

        [Required]
        [Range(YearOfBirthMinValue, YearOfBirthMaxValue)]
        public short YearOfBirth { get; set; }

        public short? YearOfDeath { get; set; }

        [StringLength(DefaultClassInfoMaxLength)]
        public string Details { get; set; }
    }
}
