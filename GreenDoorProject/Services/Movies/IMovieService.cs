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

        public MovieServiceModel Details(string id);

        public bool Edit(string id,
                string movieTitle,
                string director,
                string imagePath,
                int yearOfRelease,
                decimal ticketPrice,
                string movieDuration,
                string description);

        public IEnumerable<ActorViewModel> GetMovieActors(string movieId);
    }
}
