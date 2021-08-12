namespace GreenDoorProject.Services.Movies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Models.Cinema;

    public class MovieService : IMovieService
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
                .Skip(((currentPage < 1 ? 1 : currentPage) - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .OrderBy(a => a.Id)
                .Select(m => new MovieServiceModel
                {
                    Id = m.Id,
                    MovieTitle = m.MovieTitle,
                    Director = m.Director,
                    ImagePath = m.ImagePath,
                    MovieDuration = m.MovieDuration,
                    YearOfRelease = m.YearOfRelease,
                    TicketPrice = m.TicketPrice,
                    Rating = m.Rating,
                    Description = m.Description
                })
                .ToList();

            movieQuery = sorting switch
            {
                MovieSorting.MovieTitleAscending => movieQuery.OrderBy(m => m.MovieTitle),
                MovieSorting.MovieTitleDescending => movieQuery.OrderByDescending(m => m.MovieTitle),
                MovieSorting.DirectorNameAscending => movieQuery.OrderBy(m => m.Director),
                MovieSorting.DirectorNameDescending => movieQuery.OrderByDescending(m => m.Director),
                MovieSorting.RatingAscending => movieQuery.OrderBy(m => m.Rating),
                MovieSorting.RatingDescending => movieQuery.OrderByDescending(m => m.Rating),
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

        public MovieServiceModel Details(string id)
        {
            var getActors = GetMovieActors(id);

            var movieDetails = this.data.Movies
                  .Where(m => m.Id == id)
                  .Select(m => new MovieServiceModel
                  {
                      Id = m.Id,
                      MovieTitle = m.MovieTitle,
                      Director = m.Director,
                      ImagePath = m.ImagePath ?? string.Empty,
                      Rating = m.Rating,
                      Description = m.Description,
                      TicketPrice = m.TicketPrice,
                      YearOfRelease = m.YearOfRelease,
                      MovieDuration = m.MovieDuration,
                      Actors = getActors.ToList()
                  })
                .FirstOrDefault();

            return movieDetails;
        }

        public bool Edit(
            string id,
            string movieTitle,
            string director,
            string imagePath,
            int yearOfRelease,
            decimal ticketPrice,
            string movieDuration,
            string description)
        {
            var movieData = this.data.Movies.Find(id);

            if (movieData == null)
            {
                return false;
            }

            var movieDurationHours = int.Parse(movieDuration.Substring(0, movieDuration.IndexOf(":")));
            var movieDurationMinutes = int.Parse(movieDuration.Substring(movieDuration.IndexOf(":") + 1, 2));

            movieData.MovieTitle = movieTitle;
            movieData.Director = director;
            movieData.ImagePath = imagePath;
            movieData.YearOfRelease = yearOfRelease;
            movieData.TicketPrice = ticketPrice;
            movieData.MovieDuration = new TimeSpan(movieDurationHours, movieDurationMinutes,0);
            movieData.Description = description;

            this.data.SaveChanges();

            return true;
        }

        public string Delete(string id)
        {
            var movie = this.data.Movies.Find(id);

            this.data.Movies.Remove(movie);
            this.data.SaveChanges();

            return "The movie has been removed from the database.".ToString();
        }

        public IEnumerable<ActorViewModel> GetMovieActors(string movieId)
            => this.data.ActorMovies
                .Where(a => a.MovieId == movieId)
                .Select(a => new ActorViewModel
                {
                    Id = a.ActorId,
                    FirstName = a.Actor.FirstName,
                    LastName = a.Actor.LastName,
                    YearOfBirth = a.Actor.YearOfBirth,
                    YearOfDeath = a.Actor.YearOfDeath,
                    Details = a.Actor.Details
                })
                .ToList();
    }
}
