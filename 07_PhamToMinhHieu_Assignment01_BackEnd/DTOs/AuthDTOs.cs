using System.ComponentModel.DataAnnotations;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public short AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountEmail { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Admin", "Staff", "Lecturer"
    public int? NumericRole { get; set; }
}
