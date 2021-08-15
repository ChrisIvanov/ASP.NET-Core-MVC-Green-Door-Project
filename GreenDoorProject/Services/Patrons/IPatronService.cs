namespace GreenDoorProject.Services.Patrons
{
    public interface IPatronService
    {
        bool IsPatron(string userId);

        int CalculateTokens(string userId, decimal donationAmount);

        bool HasTokens(string patronId);

        int GetTokens(string userId);

        void UseToken(string userId);
    }
}
