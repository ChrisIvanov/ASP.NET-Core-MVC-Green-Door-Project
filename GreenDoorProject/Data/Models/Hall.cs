namespace GreenDoorProject.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Hall;

    public class Hall
    {
        public int Id { get; init; }

        [Required]
        [StringLength(HallNameMaxLength, MinimumLength = HallNameMinLength)]
        public string Name { get; set; }

        public IEnumerable<Projection> Projections { get; set; } = new List<Projection>();
    }
}