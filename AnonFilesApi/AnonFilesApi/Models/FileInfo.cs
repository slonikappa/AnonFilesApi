using Newtonsoft.Json;

namespace AnonFilesApi.Models;

[JsonObject(Title = "file")]
public class FileInfo
{
    public Metadata Metadata { get; set; } = null!;
    public Url Url { get; set; } = null!;
}