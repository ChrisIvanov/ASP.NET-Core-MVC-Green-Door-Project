namespace GreenDoorProject.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ActorMovie
    {
        [Key, Column(Order = 1)]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        [Key, Column(Order = 2)]
        public string ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
