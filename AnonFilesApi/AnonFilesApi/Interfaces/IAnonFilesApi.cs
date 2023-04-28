namespace AnonFilesApi.Interfaces;

internal interface IAnonFilesApi
{
    /// <summary>
    /// Send request to v2/file/{fileHash}/info endpoint
    /// </summary>
    /// <param name="fileHash">File hash (id) returned by anonfiles.com after uploading</param>
    /// <returns>Json as string</returns>
    Task<string> GetFileInfoAsync(string fileHash);

    /// <summary>
    /// Send request to /upload endpoint
    /// </summary>
    /// <param name="fileContent">MultipartFormDataContent with file and file name</param>
    /// <returns>Json as string</returns>
    Task<string> UploadFileAsync(MultipartFormDataContent fileContent);

    /// <summary>
    /// Get html of download file page 
    /// </summary>
    /// <param name="downloadLink">Download file link from file info</param>
    /// <returns>Html of download file page as string</returns>
    Task<string> GetDownloadPageAsync(string downloadLink);

    /// <summary>
    /// Download file by download file link from download file page
    /// </summary>
    /// <param name="downloadPageLink">Download file link from download file page</param>
    /// <returns>File as byte array</returns>
    Task<byte[]> DownloadFileByDownloadPageLinkAsync(string downloadPageLink);
}
