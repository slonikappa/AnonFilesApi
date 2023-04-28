using System.Net.Http.Headers;
using AnonFilesApi.Interfaces;

namespace AnonFilesApi.Extensions;

internal static class ApiExtensions
{
    public static async Task<string> UploadMultipartFormFileAsync(this IAnonFilesApi api, byte[] fileData, string? fileName = default)
    {
        using var memoryStream = new MemoryStream(fileData);
        using var streamContent = new StreamContent(memoryStream);

        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = fileName ?? $"File-{Guid.NewGuid()}",
        };
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        var formDataContent = new MultipartFormDataContent
        {
            streamContent
        };

        return await api.UploadFileAsync(formDataContent);
    }
}
