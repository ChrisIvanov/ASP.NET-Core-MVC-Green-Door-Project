namespace GreenDoorProject.Models.Cinema
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Movie;
    using static Data.DataConstants.Info;

    public class AddMovieViewModel
    {
        [Display(Name = "Title")]
        public string MovieTitle { get; set; }


        public string Director { get; set; }

        [Display(Name = "Year of Release")]
        public int YearOfRelease { get; set; }


        public string Description { get; set; }

        [Display(Name = "Duration")]
        public TimeSpan MovieDuration { get; set; }

        [Display(Name = "Ticket")]
        public decimal TicketPrice { get; set; }
    }
}
