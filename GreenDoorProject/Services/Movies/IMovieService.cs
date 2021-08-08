namespace GreenDoorProject.Services.Movies
{
    using GreenDoorProject.Models.Cinema;
    using System.Collections.Generic;

    public interface IMovieService
    {
        MovieQueryServiceModel All(
            string searchTerm,
            MovieSorting sorting,
            int currentPage,
            int booksPerPage);

        public MovieDetailsServiceModel Details(string id);

        public IEnumerable<string> SelectMovieActors(string movieId);
    }
}
