using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OptimistClub_SoccerRegistration.Models;

[Index("UserId", Name = "IX_Guardians_UserId")]
public partial class Guardian
{
    [Key]
    public int GuardianId { get; set; }

    public string? UserId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(25)]
    public string? Role { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(75)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    [StringLength(25)]
    public string? City { get; set; }

    [StringLength(7)]
    public string? PostalCode { get; set; }

    public DateTime DateAdded { get; set; }

    [StringLength(50)]
    public string? ElectronicSignature { get; set; }

    public bool WaiverAccepted { get; set; }

    public DateTime? WaiverAcceptedOn { get; set; }

    [InverseProperty("Guardian")]
    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    [ForeignKey("UserId")]
    [InverseProperty("Guardians")]
    public virtual AspNetUser? User { get; set; }

    [InverseProperty("Guardian")]
    public virtual Volunteer? Volunteer { get; set; }
}
