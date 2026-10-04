using System.ComponentModel.DataAnnotations;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;

public class AccountCreateDto
{
    public short? AccountId { get; set; }

    [Required(ErrorMessage = "Account name is required.")]
    [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters.")]
    public string AccountName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(70, ErrorMessage = "Email cannot exceed 70 characters.")]
    public string AccountEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required (1: Staff, 2: Lecturer).")]
    public int AccountRole { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(70, ErrorMessage = "Password cannot exceed 70 characters.")]
    public string AccountPassword { get; set; } = string.Empty;
}

public class AccountUpdateDto
{
    [Required]
    public short AccountId { get; set; }

    [Required(ErrorMessage = "Account name is required.")]
    [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters.")]
    public string AccountName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(70, ErrorMessage = "Email cannot exceed 70 characters.")]
    public string AccountEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required (1: Staff, 2: Lecturer).")]
    public int AccountRole { get; set; }

    // Optional on update
    [StringLength(70, ErrorMessage = "Password cannot exceed 70 characters.")]
    public string? AccountPassword { get; set; }
}

public class AccountResponseDto
{
    public short AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? AccountEmail { get; set; }
    public int? AccountRole { get; set; }
    public string RoleName => AccountRole switch
    {
        1 => "Staff",
        2 => "Lecturer",
        _ => "Unknown"
    };
}
