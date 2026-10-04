using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;
using _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISystemAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public AuthController(ISystemAccountRepository accountRepository, IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 1. Check default admin account from appsettings.json
        var adminEmail = _configuration["AdminAccount:Email"];
        var adminPassword = _configuration["AdminAccount:Password"];

        if (string.Equals(request.Email.Trim(), adminEmail?.Trim(), System.StringComparison.OrdinalIgnoreCase) &&
            request.Password == adminPassword)
        {
            return Ok(new LoginResponseDto
            {
                AccountId = 0,
                AccountName = "Administrator",
                AccountEmail = adminEmail!,
                Role = "Admin",
                NumericRole = 0
            });
        }

        // 2. Check accounts from database
        var account = _accountRepository.GetAccountByEmail(request.Email);
        if (account == null || account.AccountPassword != request.Password)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        // Staff role = 1, Lecturer role = 2
        string roleName = account.AccountRole switch
        {
            1 => "Staff",
            2 => "Lecturer",
            _ => "User"
        };

        return Ok(new LoginResponseDto
        {
            AccountId = account.AccountId,
            AccountName = account.AccountName ?? string.Empty,
            AccountEmail = account.AccountEmail ?? string.Empty,
            Role = roleName,
            NumericRole = account.AccountRole
        });
    }
}
