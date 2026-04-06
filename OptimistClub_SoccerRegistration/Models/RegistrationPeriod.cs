using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OptimistClub_SoccerRegistration.Models;

public partial class RegistrationPeriod
{
    [Key]
    public int PeriodId { get; set; }

    public int SeasonYear { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("Period")]
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
