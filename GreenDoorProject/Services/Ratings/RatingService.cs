namespace GreenDoorProject.Services.Ratings
{
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
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

        public RatingServiceModel RateBook(BookServiceModel model)
        {
            var newRating = OverallRating(
                model.Rating.CurrentRating,
                model.Rating.CurrentVotesCount,
                model.Rating.UserRating);

            var ratingExists = this.data.Ratings
                .Any(b => b.BookId == model.Id);

            var returnRating = new RatingServiceModel();

            if (!ratingExists)
            {
                var rating = new Rating
                {
                    BookId = model.Id,
                    CurrentRating = newRating.CurrentRating,
                    CurrentVotesCount = newRating.CurrentVotesCount,
                    UserRating = newRating.UserRating,
                    UserHasVoted = true
                };

                this.data.SaveChanges();

                returnRating.CurrentRating = rating.CurrentRating;
                returnRating.CurrentVotesCount = rating.CurrentVotesCount;
                returnRating.UserRating = rating.UserRating;
            }
            else
            {
                var changedRatingValues = this.data.Ratings
                    .Where(b => b.BookId == model.Id)
                    .Select(b => new RatingServiceModel
                    {
                        CurrentRating = b.CurrentRating,
                        CurrentVotesCount = b.CurrentVotesCount,
                        UserRating = b.UserRating
                    })
                    .FirstOrDefault();

                this.data.SaveChanges();

                returnRating = changedRatingValues;
            }

            return returnRating;
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
