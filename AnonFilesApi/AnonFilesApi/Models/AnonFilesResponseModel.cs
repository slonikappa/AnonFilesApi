namespace AnonFilesApi.Models;

public class AnonFilesResponseModel
{
    public bool Status { get; set; }
    public Data Data { get; set; } = null!;
    public Error Error { get; set; } = null!;
}
