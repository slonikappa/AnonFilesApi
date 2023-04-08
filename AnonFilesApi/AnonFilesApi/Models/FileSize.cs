using Newtonsoft.Json;

namespace AnonFilesApi.Models;

[JsonObject(Title = "size")]
public class FileSize
{
    public uint Bytes { get; set; }
    public string Readable { get; set; } = string.Empty;
}