using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptimistClub_SoccerRegistration.Data.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        public int PlayerId { get; set; }

        public int PeriodId { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        [StringLength(25)]
        public string PaymentStatus { get; set; } = "Pending";  

        [ForeignKey("PlayerId")]
        public Player Player { get; set; } = null!;

        [ForeignKey("PeriodId")]
        public RegistrationPeriod Period { get; set; } = null!;
    }
}