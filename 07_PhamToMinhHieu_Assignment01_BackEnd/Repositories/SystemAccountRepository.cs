using System.Collections.Generic;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DataAccess;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

public class SystemAccountRepository : ISystemAccountRepository
{
    public List<SystemAccount> GetAccounts() => SystemAccountDAO.Instance.GetAccounts();

    public SystemAccount? GetAccountById(short id) => SystemAccountDAO.Instance.GetAccountById(id);

    public SystemAccount? GetAccountByEmail(string email) => SystemAccountDAO.Instance.GetAccountByEmail(email);

    public void AddAccount(SystemAccount account) => SystemAccountDAO.Instance.AddAccount(account);

    public void UpdateAccount(SystemAccount account) => SystemAccountDAO.Instance.UpdateAccount(account);

    public bool DeleteAccount(short id, out string errorMessage) => SystemAccountDAO.Instance.DeleteAccount(id, out errorMessage);

    public List<SystemAccount> SearchAccounts(string keyword) => SystemAccountDAO.Instance.SearchAccounts(keyword);
}
