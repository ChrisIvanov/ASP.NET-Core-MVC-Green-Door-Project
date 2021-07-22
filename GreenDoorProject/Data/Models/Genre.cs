namespace GreenDoorProject.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Ganre;

    public class Genre
    {
        public int Id { get; init; }

        [Required]
        [StringLength(GanreNameMaxLength, MinimumLength = GanreNameMinLength)]
        public string Name { get; set; }

        public IEnumerable<Book> Books { get; init; } = new List<Book>();     
    }
}
