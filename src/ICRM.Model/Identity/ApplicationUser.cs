using System.ComponentModel.DataAnnotations.Schema;
using ICRM.Model.Enums;
using Microsoft.AspNetCore.Identity;

namespace ICRM.Model;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public short? VerificationCode { get; set; }
    public string? ImageName { get; set; }
    public bool? Activity { get; set; }

    [NotMapped]
    public string? FullName => $"{FirstName} {LastName}";
}
