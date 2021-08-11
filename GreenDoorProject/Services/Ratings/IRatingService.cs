namespace GreenDoorProject.Services.Ratings
{
    using GreenDoorProject.Services.Books.Models;
    using GreenDoorProject.Services.Movies;

    public interface IRatingService
    {
        RatingServiceModel OverallRating(
            double currentRating,
            int votesCount,
            int rating);

        RatingServiceModel RateBook(BookServiceModel model);

        RatingServiceModel RateMovie(MovieServiceModel model);
    }
}
