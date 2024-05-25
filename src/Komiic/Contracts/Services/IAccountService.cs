using System;
using System.Threading.Tasks;
using Komiic.Core.Contracts.Model;

namespace Komiic.Contracts.Services;

public interface IAccountService
{
    Account? AccountData { get;  }
    
    event EventHandler AccountChanged;
    
    Task LoadAccount();
    
    ImageLimit? ImageLimit { get;  }
    
    event EventHandler ImageLimitChanged;
    
    Task LoadImageLimit();

    Task Logout();
    
    Task<bool> Login(string username, string password);

    Task<bool> SetNextChapterMode(string mode);
}