using AnonFilesApi.Interfaces;

namespace AnonFilesApi.Implementations;

public class AnonFilesApi : IAnonFilesApi
{
    private static AnonFilesApi? _instance;
    public static AnonFilesApi Instance => _instance ??= new AnonFilesApi();

    private static readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://api.anonfiles.com/"),
    };
    
    public async Task<string> GetFileInfoAsync(string fileHash)
    {
        var response = await _httpClient.GetAsync($"v2/file/{fileHash}/info");
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> UploadFileAsync(MultipartFormDataContent fileContent)
    {
        var response = await _httpClient.PostAsync("upload", fileContent);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetDownloadPageAsync(string downloadLink)
    {
        var downloadPageResponse = await _httpClient.GetAsync(downloadLink);
        return await downloadPageResponse.Content.ReadAsStringAsync();
    }

    public async Task<byte[]> DownloadFileByDownloadPageLinkAsync(string downloadPageLink)
    {
        var fileResponse = await _httpClient.GetAsync(downloadPageLink);
        return await fileResponse.Content.ReadAsByteArrayAsync();
    }
}