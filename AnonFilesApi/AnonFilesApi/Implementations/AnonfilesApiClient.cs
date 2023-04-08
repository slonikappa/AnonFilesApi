using AnonFilesApi.Extensions;
using AnonFilesApi.Models;

using Newtonsoft.Json;

using RestEase;

using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace AnonFilesApi.Implementations;

public class AnonfilesApiClient
{
    private readonly IAnonFilesApi _api;

    public AnonfilesApiClient()
    {
        _api = RestClient.For<IAnonFilesApi>("https://api.anonfiles.com/");
    }

    public async Task<AnonFilesResponseModel> UploadFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new ArgumentException($"File {filePath} does not exist.");
        }

        var data = File.ReadAllBytes(filePath);

        if (!data.Any())
        {
            throw new FileLoadException("File Exception - File is empty or content not loaded");
        }

        var responseJson = await _api.UploadAsync(data);
        var response = JsonConvert.DeserializeObject<AnonFilesResponseModel>(responseJson);

        return response!;
    }

    public async Task<AnonFilesResponseModel> UploadFileAsync(byte[] data)
    {
        var responseJson = await _api.UploadAsync(data);
        var response = JsonConvert.DeserializeObject<AnonFilesResponseModel>(responseJson);

        return response!;
    }

    public async Task<AnonFilesResponseModel> GetAllInfo(string fileHash)
    {
        var responseJson = await _api.GetFileInfoAsync(fileHash);
        var response = JsonConvert.DeserializeObject<AnonFilesResponseModel>(responseJson);

        return response!;
    }

    public async Task<string?> GetDownloadLinkAsync(string fileHash)
    {
        var responseJson = await _api.GetFileInfoAsync(fileHash);
        var response = JsonConvert.DeserializeObject<AnonFilesResponseModel>(responseJson);

        return response?.Data.File.Url.Short;
    }

    public async Task<byte[]> DownloadFileAsync(string fileHash)
    {
        var fileDownloadLink = await GetDownloadLinkAsync(fileHash);

        using var httpClient = new HttpClient();

        var downloadPageResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, fileDownloadLink));

        var htmlDownloadPage = await downloadPageResponse!.Content.ReadAsStringAsync();

        var match = Regex.Match(htmlDownloadPage, @"https://([A-Za-z0-9]+(-[A-Za-z0-9]+)+)\.anonfiles\.com/[A-Za-z0-9]+/([A-Za-z0-9]+(-[A-Za-z0-9]+)+)/([A-Za-z0-9]+(-[A-Za-z0-9]+)+)");
        
        if (!match.Success)
        {
            return Array.Empty<byte>();
        }

        var fileResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, match.Value));

        return await fileResponse.Content.ReadAsByteArrayAsync();
    }
}
