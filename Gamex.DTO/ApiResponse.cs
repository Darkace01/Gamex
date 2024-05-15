using System.Text.Json;

namespace Gamex.DTO;

public class ApiResponse<T>
{
    public bool HasError { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public T Data { get; set; }
    public override string ToString() => JsonSerializer.Serialize(this);
    public ApiResponse() { }

    public ApiResponse(T data, int statusCode = 200, string message = "")
    {
        Data = data;
        StatusCode = statusCode;
        Message = message;
        HasError = false;
    }

    public ApiResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
        HasError = true;
    }
}
