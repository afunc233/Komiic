using System.Text.Json.Serialization;

namespace Komiic.Core.Contracts.Model;

public record LoginData
{
    [JsonPropertyName("email")] public required string Email { get; set; }
    [JsonPropertyName("password")] public required string Password { get; set; }
}