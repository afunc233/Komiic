using System.Text.Json.Serialization;

namespace Komiic.Core.Contracts.Model;

public class LoginData(string email,string password)
{
    [JsonPropertyName("email")] public string Email { get; set; } = email;
    [JsonPropertyName("password")] public string Password { get; set; } = password;

}