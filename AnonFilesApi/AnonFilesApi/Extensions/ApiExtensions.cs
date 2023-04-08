using System.Net.Http.Headers;

namespace AnonFilesApi.Extensions;

public static class ApiExtensions
{
    public static async Task<string> UploadAsync(this IAnonFilesApi api, byte[] fileData)
    {
        using var memoryStream = new MemoryStream(fileData);
        using var streamContent = new StreamContent(memoryStream);

        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = $"File-{Guid.NewGuid()}"
        };
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        var formDataContent = new MultipartFormDataContent();
        formDataContent.Add(streamContent);

        return await api.UploadFileAsync(formDataContent);
    }
}
