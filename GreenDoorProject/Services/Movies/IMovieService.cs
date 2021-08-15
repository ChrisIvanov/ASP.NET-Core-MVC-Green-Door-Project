namespace GreenDoorProject.Services.Movies
{
    using System.Collections.Generic;
    using GreenDoorProject.Models.Cinema;
    using GreenDoorProject.Services.Movies.Models;

    public interface IMovieService
    {
        MovieQueryServiceModel All(
            string searchTerm,
            MovieSorting sorting,
            int currentPage,
            int booksPerPage);

        public MovieServiceModel Details(string id);

        public bool Edit(string id,
                string movieTitle,
                string director,
                string imagePath,
                int yearOfRelease,
                string movieDuration,
                string description);

        public string Delete(string id);

        public IEnumerable<ActorViewModel> GetMovieActors(string movieId);
    }
}
