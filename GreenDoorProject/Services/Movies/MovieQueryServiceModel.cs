namespace GreenDoorProject.Services.Movies
{
    using System.Collections.Generic;
    using GreenDoorProject.Services.Books;
    
    public class MovieQueryServiceModel
    {
        public int MoviesPerPage { get; init; }

        public int CurrentPage { get; init; }

        public int TotalBooks { get; init; }

        public string ModelError { get; init; }

        public IEnumerable<MovieServiceModel> Movies { get; init; }
    }
}
