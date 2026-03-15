using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        public int ParentId { get; set; } 

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(50)]
        public string? Town { get; set; }

        [StringLength(10)]
        public string? ShirtSize { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [ForeignKey("ParentId")]
        public Parent Parent { get; set; } = null!;
        public ICollection<TeamMember> TeamAssignments { get; set; } = new List<TeamMember>();
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}