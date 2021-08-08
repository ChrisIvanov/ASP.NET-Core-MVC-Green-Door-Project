using GreenDoorProject.Services.Movies;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenDoorProject.Models.Cinema
{
    public class AllMoviesQueryModel
    {
        public string MovieTitle { get; set; }

        public string Director { get; set; }

        [Display(Name = "Search...")]
        public string SearchTerm { get; set; }

        public MovieSorting Sorting { get; set; }

        public int CurrentPage { get; set; }

        public const int MoviesPerPage = 20;

        public string ModelError { get; set; }

        public int TotalMovies { get; set; }

        public IEnumerable<MovieServiceModel> Movies { get; set; }

    }
}
