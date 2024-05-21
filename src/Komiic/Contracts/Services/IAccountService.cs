using System;
using System.Threading.Tasks;
using Komiic.Core.Contracts.Model;

namespace Komiic.Contracts.Services;

public interface IAccountService
{
    Account? AccountData { get;  }
    
    event EventHandler AccountChanged;
    
    Task LoadAccount();
    
    Task<GetImageLimitData?> GetImageLimit();

    Task Logout();
    
    Task<bool> Login(string username, string password);
}