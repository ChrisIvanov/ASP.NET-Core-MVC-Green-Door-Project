namespace GreenDoorProject.Data.Models
{
    using System;

    public class Game
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string GameTitle { get; set; } 

        public string Genre { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
