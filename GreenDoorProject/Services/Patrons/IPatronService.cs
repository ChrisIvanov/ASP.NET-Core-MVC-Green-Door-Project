namespace GreenDoorProject.Services.Patrons
{
    using GreenDoorProject.Models.Patron;
    
    public interface IPatronService
    {
        bool IsPatron(string userId);

        int CalculateTokens(string userId, decimal donationAmount);

        bool HasTokens(string patronId);

        UserPatronageViewModel GetTokens(string userId);

        void UseToken(string userId);
    }
}
