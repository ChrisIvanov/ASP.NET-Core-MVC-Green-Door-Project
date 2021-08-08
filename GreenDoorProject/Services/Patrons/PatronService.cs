namespace GreenDoorProject.Services.Patrons
{
    using System.Linq;
    using GreenDoorProject.Data;
    using Microsoft.AspNetCore.Identity;

    public class PatronService : IPatronService
    {
        private readonly GreenDoorProjectDbContext data;

        public PatronService(GreenDoorProjectDbContext data)
            => this.data = data;

        public int CalculateTokens(string userId, int donationAmount)
        {
            var tokens = this.data.Patrons
                .Where(u => u.Id == userId)
                .Select(p => p.Token)
                .FirstOrDefault();

            if (donationAmount  >= 5 && donationAmount  < 9.99) tokens += 2;
            else if (donationAmount >= 10 && donationAmount < 19.99) tokens += 5;
            else if (donationAmount >= 20 && donationAmount < 29.99) tokens += 12;
            else if (donationAmount >= 30 && donationAmount < 39.99) tokens += 20;
            else if (donationAmount >= 40 && donationAmount < 49.99) tokens += 30;
            else if (donationAmount >= 50 && donationAmount < 59.99) tokens += 45;

            this.data.SaveChanges();

            return tokens;
        }

        public int GetTokens(string userId)
            => this.data.Patrons.Where(u => u.Id == userId)
            .Select(t => t.Token).FirstOrDefault();

        public void UseToken(string userId)
        {
            var tokens = this.data.Patrons
                .Where(u => u.Id == userId)
                .Select(p => p.Token)
                .FirstOrDefault();

            tokens--;
        }

        public bool IsPatron(string userId)
            => this.data.Patrons
                .Any(p => p.UserId == userId);
    }
}
