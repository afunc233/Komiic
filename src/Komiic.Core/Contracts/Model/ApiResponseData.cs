namespace Komiic.Core.Contracts.Model;

public record ApiResponseData<T>
{
    public string? ErrorMessage { get; init; }

    public T? Data { get; init; }
}