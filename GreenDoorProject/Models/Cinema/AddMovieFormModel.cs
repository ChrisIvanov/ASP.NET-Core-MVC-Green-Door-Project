﻿namespace GreenDoorProject.Models.Cinema
{
    using GreenDoorProject.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Info;
    using static Data.DataConstants.Movie;
    
    public class AddMovieFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string MovieTitle { get; set; }

        [Required]
        [StringLength(DirectorNameMaxLength, MinimumLength = DirectorNameMinLength)]
        public string Director { get; set; }

        [Required]
        [Range(YearOfReleaseMinValue, YearOfReleaseMaxValue)]
        public int YearOfRelease { get; set; }

        [StringLength(DefaultClassInfoMaxLength)]
        public string Description { get; set; }

        public string MovieDuration { get; set; }

        public string RatingId { get; set; }
        [Range(0, 5)]
        public Rating Rating { get; set; }

        [Required]
        [Range(TicketPriceMaxValue, TicketPriceMinValue)]
        public decimal TicketPrice { get; set; }

        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

        public ICollection<Projection> Projections { get; set; } = new List<Projection>();
    }
}
