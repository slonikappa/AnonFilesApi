namespace AnonFilesApi.Helpers;

public static class FileHelpers
{
    public static string GetLoadedFileName(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        return fileInfo.Name;
    }
}