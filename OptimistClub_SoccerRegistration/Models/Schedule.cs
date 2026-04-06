using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OptimistClub_SoccerRegistration.Models;

[Index("TeamId", Name = "IX_Schedules_TeamId")]
public partial class Schedule
{
    [Key]
    public int ScheduleId { get; set; }

    public int TeamId { get; set; }

    [StringLength(50)]
    public string? Location { get; set; }

    public DateTime GameDate { get; set; }

    public TimeOnly GameTime { get; set; }

    [ForeignKey("TeamId")]
    [InverseProperty("Schedules")]
    public virtual Team Team { get; set; } = null!;
}
