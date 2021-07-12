namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string GameTitle { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
