namespace GreenDoorProject.Models.Patron
{
    using System;

    public class UserPatronageViewModel
    {
        public int Id { get; init; }

        public string UserId { get; set; }

        public decimal Donations { get; set; }

        public int Tokens { get; set; }

        public DateTime PatronSince { get; init; }
    }
}
