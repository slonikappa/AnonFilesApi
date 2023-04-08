namespace AnonFilesApi.Models;

public class Metadata
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public FileSize Size { get; set; } = null!;
}