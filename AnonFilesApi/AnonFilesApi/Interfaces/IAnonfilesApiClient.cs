using AnonFilesApi.Models;

namespace AnonFilesApi.Interfaces;

public interface IAnonfilesApiClient
{
    Task<AnonFilesResponseModel> UploadFileAsync(byte[] data);
    Task<AnonFilesResponseModel> UploadFileAsync(string filePath);
    Task<AnonFilesResponseModel> GetAllInfo(string fileHash);
    Task<string?> GetDownloadLinkAsync(string fileHash);
    Task<byte[]> DownloadFileByHashAsync(string fileHash);
    Task<byte[]> DownloadFileByDownloadLinkAsync(string downloadLink);
}
