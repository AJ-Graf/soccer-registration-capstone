using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class TeamMember
    {
        [Key]
        public int TeamMemberId { get; set; }

        public int TeamId { get; set; }  

        public int? PlayerId { get; set; } 

        public int? VolunteerId { get; set; }  

        [StringLength(25)]
        public string? Role { get; set; } 

        public DateTime AssignedDate { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; } = null!;

        [ForeignKey("PlayerId")]
        public Player? Player { get; set; }

        [ForeignKey("VolunteerId")]
        public Volunteer? Volunteer { get; set; }
    }
}