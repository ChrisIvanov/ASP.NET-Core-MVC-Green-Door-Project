namespace GreenDoorProject.Services.Ratings
{
    using GreenDoorProject.Services.Books.Models;
    
    public interface IRatingService
    {
        RatingServiceModel OverallRating(
            double currentRating,
            int votesCount,
            int rating);

        RatingServiceModel RateBook(BookServiceModel model);
    }
}
