using System.ComponentModel.DataAnnotations;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamName { get; set; } = string.Empty;

        public int SeasonYear { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}