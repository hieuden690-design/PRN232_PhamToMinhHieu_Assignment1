using System.Collections.Generic;
using System.Linq;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.DataAccess;

public class SystemAccountDAO
{
    private static SystemAccountDAO? _instance;
    private static readonly object _lock = new object();

    private SystemAccountDAO() { }

    public static SystemAccountDAO Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new SystemAccountDAO();
                }
                return _instance;
            }
        }
    }

    public List<SystemAccount> GetAccounts()
    {
        using var context = new FUNewsManagementDbContext();
        return context.SystemAccounts.ToList();
    }

    public SystemAccount? GetAccountById(short id)
    {
        using var context = new FUNewsManagementDbContext();
        return context.SystemAccounts.Find(id);
    }

    public SystemAccount? GetAccountByEmail(string email)
    {
        using var context = new FUNewsManagementDbContext();
        return context.SystemAccounts
            .FirstOrDefault(a => a.AccountEmail != null && a.AccountEmail.ToLower() == email.Trim().ToLower());
    }

    public short GetNextAccountId()
    {
        using var context = new FUNewsManagementDbContext();
        if (!context.SystemAccounts.Any())
        {
            return 1;
        }
        return (short)(context.SystemAccounts.Max(a => a.AccountId) + 1);
    }

    public void AddAccount(SystemAccount account)
    {
        using var context = new FUNewsManagementDbContext();
        if (account.AccountId == 0)
        {
            account.AccountId = GetNextAccountId();
        }
        context.SystemAccounts.Add(account);
        context.SaveChanges();
    }

    public void UpdateAccount(SystemAccount account)
    {
        using var context = new FUNewsManagementDbContext();
        var existing = context.SystemAccounts.Find(account.AccountId);
        if (existing != null)
        {
            existing.AccountName = account.AccountName;
            existing.AccountEmail = account.AccountEmail;
            existing.AccountRole = account.AccountRole;
            if (!string.IsNullOrEmpty(account.AccountPassword))
            {
                existing.AccountPassword = account.AccountPassword;
            }
            context.SaveChanges();
        }
    }

    public bool DeleteAccount(short id, out string errorMessage)
    {
        using var context = new FUNewsManagementDbContext();
        // Check if this account has created any news articles
        bool hasCreatedArticles = context.NewsArticles.Any(n => n.CreatedById == id);
        if (hasCreatedArticles)
        {
            errorMessage = "Cannot delete this account because it has created one or more news articles.";
            return false;
        }

        var account = context.SystemAccounts.Find(id);
        if (account == null)
        {
            errorMessage = "Account not found.";
            return false;
        }

        context.SystemAccounts.Remove(account);
        context.SaveChanges();
        errorMessage = string.Empty;
        return true;
    }

    public List<SystemAccount> SearchAccounts(string keyword)
    {
        using var context = new FUNewsManagementDbContext();
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return GetAccounts();
        }

        keyword = keyword.Trim().ToLower();
        return context.SystemAccounts
            .Where(a => (a.AccountName != null && a.AccountName.ToLower().Contains(keyword)) ||
                        (a.AccountEmail != null && a.AccountEmail.ToLower().Contains(keyword)))
            .ToList();
    }
}
