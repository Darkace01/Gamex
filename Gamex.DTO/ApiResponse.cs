using System.Text.Json;

namespace Gamex.DTO;

public class ApiResponse<T>
{
    public ApiResponse() { }
    public bool HasError { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public T Data { get; set; }
    public override string ToString() => JsonSerializer.Serialize(this);
}
