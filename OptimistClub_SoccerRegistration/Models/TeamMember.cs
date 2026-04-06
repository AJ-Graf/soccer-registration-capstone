using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OptimistClub_SoccerRegistration.Models;

[Index("PlayerId", Name = "IX_TeamMembers_PlayerId")]
[Index("TeamId", Name = "IX_TeamMembers_TeamId")]
[Index("VolunteerId", Name = "IX_TeamMembers_VolunteerId")]
public partial class TeamMember
{
    [Key]
    public int TeamMemberId { get; set; }

    public int TeamId { get; set; }

    public int? PlayerId { get; set; }

    public int? VolunteerId { get; set; }

    [StringLength(25)]
    public string? Role { get; set; }

    public DateTime AssignedDate { get; set; }

    [ForeignKey("PlayerId")]
    [InverseProperty("TeamMembers")]
    public virtual Player? Player { get; set; }

    [ForeignKey("TeamId")]
    [InverseProperty("TeamMembers")]
    public virtual Team Team { get; set; } = null!;

    [ForeignKey("VolunteerId")]
    [InverseProperty("TeamMembers")]
    public virtual Volunteer? Volunteer { get; set; }
}
