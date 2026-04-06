using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OptimistClub_SoccerRegistration.Models;

public partial class Team
{
    [Key]
    public int TeamId { get; set; }

    [StringLength(50)]
    public string TeamName { get; set; } = null!;

    public int SeasonYear { get; set; }

    [StringLength(10)]
    public string? AgeGroup { get; set; }

    [InverseProperty("Team")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    [InverseProperty("Team")]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
