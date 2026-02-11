using System.ComponentModel.DataAnnotations;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class RegistrationPeriod
    {
        [Key]
        public int PeriodId { get; set; }

        public int SeasonYear { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}