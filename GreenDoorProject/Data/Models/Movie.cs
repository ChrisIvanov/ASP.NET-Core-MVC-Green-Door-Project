namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Description;
    using static Data.DataConstants.Movie;

    public class Movie
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string MovieTitle { get; set; }

        [Required]
        [StringLength(DirectorNameMaxLength, MinimumLength = DirectorNameMinLength)]
        public string Director { get; set; }

        [Required]
        [Range(YearOfReleaseMinValue, YearOfReleaseMaxValue)]
        public int YearOfRelease { get; set; }

        [StringLength(DefaultDescriptionMaxLength)]
        public string Description { get; set; }

        public TimeSpan MovieDuration { get; set; }

        [Required]
        [Range(TicketPriceMaxValue, TicketPriceMinValue)]
        public decimal TicketPrice { get; set; }

        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

        public ICollection<Projection> Projections { get; init; } = new List<Projection>();
    }
}
