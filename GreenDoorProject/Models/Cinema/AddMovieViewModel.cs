namespace GreenDoorProject.Models.Cinema
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;
    
    public class AddMovieViewModel
    {
        [Required]
        [MinLength(MovieTitleMinLength)]
        [MaxLength(MovieTitleMaxLength)]
        public string MovieTitle { get; set; }

        [Required]
        [MinLength(MovieDirectorNameMinLength)]
        [MaxLength(MovieDirectorNameMaxLength)]
        public string Director { get; set; }

        [Required]
        [MinLength(MovieYearOfReleaseMinValue)]
        [MaxLength(MovieYearOfReleaseMaxValue)]
        public int YearOfRelease { get; set; }

        [MaxLength(DefaultDescriptionMaxLength)]
        public string Description { get; set; }


        public TimeSpan MovieDuration { get; set; }

        [Required]
        [Range(5.00, 25.00)]
        public decimal TicketPrice { get; set; }
    }
}
