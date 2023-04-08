using Newtonsoft.Json;

namespace AnonFilesApi.Models;

[JsonObject(Title = "error")]
public class Error
{
    public string Message { get; set; } = string.Empty;
    public ErrorType Type { get; set; }
    public int Code { get; set; }
}