﻿using System.Threading.Tasks;

namespace Komiic.Contracts.Services;

public interface ITokenService
{
    Task<string?> GetToken();
    
    Task SetToken(string token,bool save = true);
    
    Task ClearToken();
    
    Task LoadToken();
    
    Task RefreshToken();
}