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

        public int CalculateTokens(string userId, decimal donations)
        {
            var tokens = 0;

            if (donations >= 5 && donations < 10) tokens = 2;
            else if (donations >= 10 && donations < 20) tokens = 5;
            else if (donations >= 20 && donations < 30) tokens = 12;
            else if (donations >= 30 && donations < 40) tokens = 20;
            else if (donations >= 40 && donations < 50) tokens = 35;
            else if (donations >= 50 && donations < 60) tokens = 45;

            return tokens;
        }

        public bool HasTokens(string patronId)
        {
            var tokens = GetTokens(patronId);

            if (tokens > 0)
            {
                tokens -= 1;
                return true;
            }

            return false;
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
