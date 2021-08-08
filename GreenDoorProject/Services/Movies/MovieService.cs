namespace GreenDoorProject.Services.Movies
{
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Models.Cinema;

    public class MovieService : IMoviesService
    {
        private readonly GreenDoorProjectDbContext data;

        public MovieService(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        public MovieQueryServiceModel All(
            string searchTerm,
            MovieSorting sorting,
            int currentPage,
            int moviesPerPage)
        {
            var movieQuery = this.data.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                movieQuery = movieQuery.Where(b =>
                    b.MovieTitle.ToLower().Contains(searchTerm.ToLower())
                    || b.Director.Contains(searchTerm.ToLower())
                    || b.Description.Contains(searchTerm.ToLower()));
            }

            var totalBooks = movieQuery.Count();

            var movies = movieQuery
                .Skip((currentPage - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .OrderBy(a => a.Id)
                .Select(m => new MovieServiceModel
                {
                    MovieTitle = m.MovieTitle,
                    Director = m.Director,
                    ImagePath = m.ImagePath
                })
                .ToList();

            movieQuery = sorting switch
            {
                MovieSorting.MovieTitleAscending => movieQuery.OrderBy(m => m.MovieTitle),
                MovieSorting.MovieTitleDescending => movieQuery.OrderByDescending(m => m.MovieTitle),
                MovieSorting.DirectorNameAscending => movieQuery.OrderBy(m => m.Director),
                MovieSorting.DirectorNameDescending => movieQuery.OrderByDescending(m => m.Director),
                MovieSorting.RatingAscending => movieQuery.OrderByDescending(m => m.Rating.CurrentRating),
                MovieSorting.RatingDescending => movieQuery.OrderByDescending(m => m.Rating.CurrentRating),
                MovieSorting.ProjectionDateAscending => movieQuery.OrderBy(m => m.Projections.Select(m => m.TimeOfProjection)),
                MovieSorting.ProjectionDateDescending => movieQuery.OrderByDescending(m => m.Projections.Select(m => m.TimeOfProjection)),
                _ => movieQuery.OrderByDescending(b => b.Id)
            };


            return new MovieQueryServiceModel
            {
                TotalBooks = totalBooks,
                CurrentPage = currentPage,
                MoviesPerPage = moviesPerPage,
                Movies = movies
            };
        }

        public MovieDetailsServiceModel Details(string id)
        {
            var movieDetails = this.data.Movies
                  .Where(m => m.Id == id)
                  .Select(m => new MovieDetailsServiceModel
                  {
                      Id = m.Id,
                      MovieTitle = m.MovieTitle,
                      Director = m.Director,
                      ImagePath = m.ImagePath,
                      Rating = m.Rating.CurrentRating,
                      Description = m.Description,
                      TicketPrice = m.TicketPrice,
                      YearOfRelease = m.YearOfRelease,
                      MovieDuration = m.MovieDuration.ToString(),
                      Actors = SelectMovieActors(m.Id)
                  })
                .FirstOrDefault();

            return movieDetails;
        }

        public IEnumerable<string> SelectMovieActors(string movieId)
            => this.data
                .ActorMovies
                .Where(a => a.MovieId == movieId)
                .Select(a => a.Actor.FirstName + " " + a.Actor.LastName)
                .ToList();
    }
}
