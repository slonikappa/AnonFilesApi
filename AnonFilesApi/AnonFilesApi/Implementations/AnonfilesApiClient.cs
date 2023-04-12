using AnonFilesApi.Extensions;
using AnonFilesApi.Interfaces;
using AnonFilesApi.Models;

using Newtonsoft.Json;
using System.Text.RegularExpressions;
using AnonFilesApi.Exceptions;

namespace AnonFilesApi.Implementations;

public class AnonfilesApiClient : IAnonfilesApiClient
{
    private readonly IAnonFilesApi _api;
    
    public AnonfilesApiClient()
    {
        _api = AnonFilesApi.Instance;
    }

    /// <summary>
    /// Upload file to anonfiles using file path
    /// </summary>
    /// <param name="filePath">Full path to the file</param>
    /// <returns cref="AnonFilesResponseModel">AnonFilesResponseModel, that represents file information data</returns>
    /// <exception cref="ArgumentException">File not exists</exception>
    /// <exception cref="FileLoadException">File is empty or not loaded correctly</exception>
    public async Task<AnonFilesResponseModel> UploadFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new ArgumentException($"File {filePath} does not exist.");
        }

        var data = await File.ReadAllBytesAsync(filePath);
        
        if (!data.Any())
        {
            throw new FileLoadException("File Exception - File is empty or content not loaded");
        }

        return await UploadFileAsync(data);
    }

    /// <summary>
    /// Upload file to anonfiles using file bytes
    /// </summary>
    /// <param name="data">File as byte array</param>
    /// <returns cref="AnonFilesResponseModel">AnonFilesResponseModel, that represents file information data</returns>
    public async Task<AnonFilesResponseModel>UploadFileAsync(byte[] data)
    {
        var responseJson = await _api.UploadMultipartFormFileAsync(data);
        var response = JsonConvert.DeserializeObject<AnonFilesResponseModel>(responseJson);

        return response!;
    }

    /// <summary>
    /// Get all information about file
    /// </summary>
    /// <param name="fileHash">File hash (id)</param>
    /// <returns cref="AnonFilesResponseModel">Model, that represents file information data</returns>
    public async Task<AnonFilesResponseModel> GetAllInfo(string fileHash)
    {
        var responseJson = await _api.GetFileInfoAsync(fileHash);
        var response = JsonConvert.DeserializeObject<AnonFilesResponseModel>(responseJson);

        return response!;
    }

    /// <summary>
    /// Get file download link
    /// </summary>
    /// <param name="fileHash">File hash (id)</param>
    /// <returns>Download link as string</returns>
    public async Task<string?> GetDownloadLinkAsync(string fileHash)
    {
        var responseJson = await _api.GetFileInfoAsync(fileHash);
        var response = JsonConvert.DeserializeObject<AnonFilesResponseModel>(responseJson);

        return response?.Data.File.Url.Short;
    }

    /// <summary>
    /// Download file by file hash
    /// </summary>
    /// <param name="fileHash">File hash (id)</param>
    /// <returns>File as byte array</returns>
    /// <exception cref="InvalidDownloadLinkException">File download link is empty or doesn't exist</exception>
    public async Task<byte[]> DownloadFileByHashAsync(string fileHash)
    {
        var fileDownloadLink = await GetDownloadLinkAsync(fileHash);

        if (string.IsNullOrEmpty(fileDownloadLink))
        {
            throw new InvalidDownloadLinkException($"File download link is empty or doesn't exist. Download link: {fileDownloadLink}");
        }

        return await DownloadFileByDownloadLinkAsync(fileDownloadLink);
    }

    /// <summary>
    /// Download file by download link
    /// </summary>
    /// <param name="downloadLink">Download link</param>
    /// <returns>File as byte array or empty array if file download link is not found on download file page</returns>
    public async Task<byte[]> DownloadFileByDownloadLinkAsync(string downloadLink)
    {
        using var httpClient = new HttpClient();

        var downloadPageHtml = await _api.GetDownloadPageAsync(downloadLink);

        var match = Regex.Match(downloadPageHtml, @"https://([A-Za-z0-9]+(-[A-Za-z0-9]+)+)\.anonfiles\.com/[A-Za-z0-9]+/([A-Za-z0-9]+(-[A-Za-z0-9]+)+)/([A-Za-z0-9]+(-[A-Za-z0-9]+)+)");

        if (!match.Success)
        {
            return Array.Empty<byte>();
        }

        return await _api.DownloadFileByDownloadPageLinkAsync(match.Value);
    }
}
