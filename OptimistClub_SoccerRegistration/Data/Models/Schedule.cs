using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        public int TeamId { get; set; }

        [StringLength(50)]
        public string? Location { get; set; }

        public DateTime GameDate { get; set; }

        public TimeSpan GameTime { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; } = null!;
    }
}