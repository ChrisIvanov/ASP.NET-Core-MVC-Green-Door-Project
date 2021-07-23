namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Game;
    using static DataConstants.Info;

    public class Game
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string GameTitle { get; set; }

        [Required]
        [StringLength(GameGanreNameMaxLength, MinimumLength = GameGanreNameMinLength)]
        public string Genre { get; set; }

        [Required]
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }

        [StringLength(DefaultClassInfoMaxLength)]
        public string Description { get; set; }
        
        [Required]
        public string AdminId { get; set; }
        public Admin Admin { get; init; }
    }
}
