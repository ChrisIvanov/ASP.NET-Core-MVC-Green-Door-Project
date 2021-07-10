namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public string CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
