using AnonFilesApi.Models;

namespace AnonFilesApi.Interfaces;

public interface IAnonfilesApiClient
{
    Task<AnonFilesResponseModel> UploadFileAsync(byte[] data, string? fileName = default);
    Task<AnonFilesResponseModel> UploadFileAsync(string filePath, string? fileName = default);
    Task<AnonFilesResponseModel> GetAllInfo(string fileHash);
    Task<string?> GetDownloadLinkAsync(string fileHash);
    Task<byte[]> DownloadFileByHashAsync(string fileHash);
    Task<byte[]> DownloadFileByDownloadLinkAsync(string downloadLink);
}
