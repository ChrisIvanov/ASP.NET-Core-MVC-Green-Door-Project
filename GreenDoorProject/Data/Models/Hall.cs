using System.Collections.Generic;

namespace GreenDoorProject.Data.Models
{
    public class Hall
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public IEnumerable<Projection> Projections { get; set; } = new List<Projection>();
    }
}