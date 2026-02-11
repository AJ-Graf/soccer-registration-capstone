using System.ComponentModel.DataAnnotations;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class Guardian
    {
        [Key]
        public int GuardianId { get; set; }

        public string? UserId { get; set; }  

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(25)]
        public string? Role { get; set; }  

        [StringLength(11)]
        public string? PhoneNumber { get; set; }

        [StringLength(30)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? Address { get; set; }

        [StringLength(25)]
        public string? City { get; set; }

        [StringLength(7)]
        public string? PostalCode { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public ApplicationUser? User { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public Volunteer? Volunteer { get; set; }
    }
}