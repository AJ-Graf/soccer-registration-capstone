using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OptimistClub_SoccerRegistration.Models;

[Index("GuardianId", Name = "IX_Players_GuardianId")]
public partial class Player
{
    [Key]
    public int PlayerId { get; set; }

    public int GuardianId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    public string? MedicalInfo { get; set; }

    public DateTime DateAdded { get; set; }

    [StringLength(10)]
    public string? ShirtSize { get; set; }

    [StringLength(50)]
    public string? Town { get; set; }

    [ForeignKey("GuardianId")]
    [InverseProperty("Players")]
    public virtual Guardian Guardian { get; set; } = null!;

    [InverseProperty("Player")]
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    [InverseProperty("Player")]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
