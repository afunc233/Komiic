namespace Komiic.Core.Contracts.Models;

public record ApiResponseData<T>
{
    public string? ErrorMessage { get; init; }

    public T? Data { get; init; }
}