using Newtonsoft.Json.Linq;
using RestEase;

namespace AnonFilesApi;

public interface IAnonFilesApi
{
    [Get("v2/file/{fileHash}/info")]
    Task<string> GetFileInfoAsync([Path] string fileHash);

    [Post("/upload")]
    Task<string> UploadFileAsync([Body] MultipartFormDataContent fileContent);
}
