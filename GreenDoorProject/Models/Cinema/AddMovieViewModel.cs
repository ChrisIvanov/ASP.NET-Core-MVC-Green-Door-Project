namespace GreenDoorProject.Models.Cinema
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Movie;
    using static Data.DataConstants.Info;
    
    public class AddMovieViewModel
    {
        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string MovieTitle { get; set; }

        [Required]
        [MinLength(DirectorNameMinLength)]
        [MaxLength(DirectorNameMaxLength)]
        public string Director { get; set; }

        [Required]
        [MinLength(YearOfReleaseMaxValue)]
        public int YearOfRelease { get; set; }

        [MaxLength(DefaultClassInfoMaxLength)]
        public string Description { get; set; }

        public TimeSpan MovieDuration { get; set; }

        [Required]
        [Range(5.00, 25.00)]
        public decimal TicketPrice { get; set; }
    }
}
