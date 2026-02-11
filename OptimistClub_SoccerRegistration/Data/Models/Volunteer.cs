using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerId { get; set; }

        public int GuardianId { get; set; } 

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(25)]
        public string? Role { get; set; } 

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [ForeignKey("GuardianId")]
        public Guardian Guardian { get; set; } = null!;
        public ICollection<TeamMember> TeamAssignments { get; set; } = new List<TeamMember>();
    }
}