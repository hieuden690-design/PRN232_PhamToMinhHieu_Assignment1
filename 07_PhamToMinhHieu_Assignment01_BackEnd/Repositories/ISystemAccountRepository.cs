using System.Collections.Generic;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

public interface ISystemAccountRepository
{
    List<SystemAccount> GetAccounts();
    SystemAccount? GetAccountById(short id);
    SystemAccount? GetAccountByEmail(string email);
    void AddAccount(SystemAccount account);
    void UpdateAccount(SystemAccount account);
    bool DeleteAccount(short id, out string errorMessage);
    List<SystemAccount> SearchAccounts(string keyword);
}
