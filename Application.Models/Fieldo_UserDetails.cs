using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public partial class Fieldo_UserDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LanguageId { get; set; }
    public int? ServiceCategoryId { get; set; }
    public string? StripeCustomerId { get; set; }
    public string? SquareCustomerId { get; set; }
    public float YearOfExperience { get; set; }
    public int? DomainId { get; set; }

    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    public Fieldo_Role Role { get; set; }
    [ForeignKey("ServiceCategoryId")]
    public Fieldo_RequestCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string Password { get; set; }
    //public string Status { get; set; }
    public bool IsOnline { get; set; }
    public bool IsActive { get; set; }
    public string? ProfileUrl { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiry { get; set; }
    public string? CountryCode { get; set; }
}
