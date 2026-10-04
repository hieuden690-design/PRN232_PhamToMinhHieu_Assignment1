using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;
using _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ISystemAccountRepository _accountRepository;

    public AccountsController(ISystemAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AccountResponseDto>> GetAll([FromQuery] string? search)
    {
        var accounts = string.IsNullOrWhiteSpace(search)
            ? _accountRepository.GetAccounts()
            : _accountRepository.SearchAccounts(search);

        var response = accounts.Select(a => new AccountResponseDto
        {
            AccountId = a.AccountId,
            AccountName = a.AccountName,
            AccountEmail = a.AccountEmail,
            AccountRole = a.AccountRole
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public ActionResult<AccountResponseDto> GetById(short id)
    {
        var account = _accountRepository.GetAccountById(id);
        if (account == null)
        {
            return NotFound(new { message = $"Account with ID {id} not found." });
        }

        return Ok(new AccountResponseDto
        {
            AccountId = account.AccountId,
            AccountName = account.AccountName,
            AccountEmail = account.AccountEmail,
            AccountRole = account.AccountRole
        });
    }

    [HttpPost]
    public IActionResult Create([FromBody] AccountCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingEmail = _accountRepository.GetAccountByEmail(dto.AccountEmail);
        if (existingEmail != null)
        {
            return BadRequest(new { message = "Email already exists in the system." });
        }

        var account = new SystemAccount
        {
            AccountId = dto.AccountId ?? 0,
            AccountName = dto.AccountName,
            AccountEmail = dto.AccountEmail,
            AccountRole = dto.AccountRole,
            AccountPassword = dto.AccountPassword
        };

        _accountRepository.AddAccount(account);
        return CreatedAtAction(nameof(GetById), new { id = account.AccountId }, new AccountResponseDto
        {
            AccountId = account.AccountId,
            AccountName = account.AccountName,
            AccountEmail = account.AccountEmail,
            AccountRole = account.AccountRole
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(short id, [FromBody] AccountUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != dto.AccountId)
        {
            return BadRequest(new { message = "ID in URL does not match ID in body." });
        }

        var existing = _accountRepository.GetAccountById(id);
        if (existing == null)
        {
            return NotFound(new { message = $"Account with ID {id} not found." });
        }

        // Check if email changed and duplicates another
        var duplicateEmail = _accountRepository.GetAccountByEmail(dto.AccountEmail);
        if (duplicateEmail != null && duplicateEmail.AccountId != id)
        {
            return BadRequest(new { message = "Email is already taken by another account." });
        }

        existing.AccountName = dto.AccountName;
        existing.AccountEmail = dto.AccountEmail;
        existing.AccountRole = dto.AccountRole;
        if (!string.IsNullOrWhiteSpace(dto.AccountPassword))
        {
            existing.AccountPassword = dto.AccountPassword;
        }

        _accountRepository.UpdateAccount(existing);
        return Ok(new { message = "Account updated successfully." });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(short id)
    {
        bool success = _accountRepository.DeleteAccount(id, out string errorMessage);
        if (!success)
        {
            return BadRequest(new { message = errorMessage });
        }

        return Ok(new { message = "Account deleted successfully." });
    }
}
