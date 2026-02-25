using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        public int GuardianId { get; set; } 

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

        public string? MedicalInfo { get; set; } // Medical info in not part of the form. This is left in but can be added as a form field in the future.

        [StringLength(10)]
        public string? ShirtSize { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [ForeignKey("GuardianId")]
        public Guardian Guardian { get; set; } = null!;
        public ICollection<TeamMember> TeamAssignments { get; set; } = new List<TeamMember>();
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}