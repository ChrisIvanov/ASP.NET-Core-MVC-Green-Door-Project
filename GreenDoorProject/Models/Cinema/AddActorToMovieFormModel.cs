namespace GreenDoorProject.Models.Cinema
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddActorToMovieFormModel
    {
        [Required]
        public string ActorId { get; set; }
        public IEnumerable<ActorViewModel> Actors { get; set; }

        [Required]
        public string MovieId { get; set; }
        public IEnumerable<MovieViewModel> Movies { get; set; }
    }
}
