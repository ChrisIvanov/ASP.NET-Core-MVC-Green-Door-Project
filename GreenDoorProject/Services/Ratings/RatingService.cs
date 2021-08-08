namespace GreenDoorProject.Services.Ratings
{
    using GreenDoorProject.Data;
    using GreenDoorProject.Services.Books.Models;
    using System.Linq;

    public class RatingService : IRatingService
    {
        private readonly GreenDoorProjectDbContext data;

        public RatingService(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        public RatingServiceModel OverallRating(
            double currentRating, 
            int votesCount, 
            int rating)
        {
            double totalScore = currentRating * votesCount;

            votesCount++;

            totalScore += rating;

            currentRating = totalScore / votesCount;

            return new RatingServiceModel
            {
                CurrentRating = currentRating,
                CurrentVotesCount = votesCount,
                UserRating = rating
            };
        }

        

        //public double RateMovie(MovieDetailsService model, int rating)
        //{
        //    var rating = this.data.Ratings
        //        .Where(r => r.)

        //    return 1.00d;
        //}

        //public double RateMusicAlubm(MusicDetailsServiceModel model, int rating)
        //{
        //    var rating = this.data.Ratings
        //        .Where(r => r.)

        //    return 1.00d;
        //}
    }


}
