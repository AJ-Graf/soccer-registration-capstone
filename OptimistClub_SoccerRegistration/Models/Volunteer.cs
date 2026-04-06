using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OptimistClub_SoccerRegistration.Models;

[Index("GuardianId", Name = "IX_Volunteers_GuardianId", IsUnique = true)]
public partial class Volunteer
{
    [Key]
    public int VolunteerId { get; set; }

    public int GuardianId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(25)]
    public string? Role { get; set; }

    public DateTime DateAdded { get; set; }

    public bool CriminalCheckCompleted { get; set; }

    [StringLength(10)]
    public string? ShirtSize { get; set; }

    [ForeignKey("GuardianId")]
    [InverseProperty("Volunteer")]
    public virtual Guardian Guardian { get; set; } = null!;

    [InverseProperty("Volunteer")]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
