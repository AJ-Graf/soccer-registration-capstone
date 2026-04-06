using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OptimistClub_SoccerRegistration.Models;

[Index("PeriodId", Name = "IX_Registrations_PeriodId")]
[Index("PlayerId", Name = "IX_Registrations_PlayerId")]
public partial class Registration
{
    [Key]
    public int RegistrationId { get; set; }

    public int PlayerId { get; set; }

    public int? PeriodId { get; set; }

    public DateTime RegisteredAt { get; set; }

    [StringLength(25)]
    public string PaymentStatus { get; set; } = null!;

    [ForeignKey("PeriodId")]
    [InverseProperty("Registrations")]
    public virtual RegistrationPeriod? Period { get; set; }

    [ForeignKey("PlayerId")]
    [InverseProperty("Registrations")]
    public virtual Player Player { get; set; } = null!;
}
