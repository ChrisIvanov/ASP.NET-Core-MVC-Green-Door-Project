namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Game;
    using static Data.DataConstants.Description;

    public class Game
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string GameTitle { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }

        [StringLength(DefaultDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
