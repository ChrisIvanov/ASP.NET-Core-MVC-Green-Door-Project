namespace GreenDoorProject.Services.Patrons
{
    public interface IPatronService
    {
        bool IsPatron(string userId);

        int CalculateTokens(string userId, int donationAmount);

        int GetTokens(string userId);

        void UseToken(string userId);
    }
}
